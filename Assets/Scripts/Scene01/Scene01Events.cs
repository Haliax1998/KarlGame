using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene01Events : MonoBehaviour
{

    public GameObject wawo;
    public GameObject wawi;
    public GameObject textBox;

    [SerializeField] string textToSpeak;
    [SerializeField] int currentTextLength;
    [SerializeField] int textLength;
    [SerializeField] GameObject mainTextObject;
    [SerializeField] GameObject charName;
    [SerializeField] GameObject nextButton;
    [SerializeField] int eventPos = 0;
    [SerializeField] GameObject fadeIn;
    [SerializeField] GameObject fadeOut;

    List<StoryBlock> storyBlocks;

    void Update()
    {
        textLength = TextCreator.charCount;
    }

    void Start()
    {
        // Carga todas las líneas de la historia
        storyBlocks = ScriptLoaderTxtAsXml.LoadStoryBlocks();
        StartCoroutine(EventStarter());
    }

    IEnumerator EventStarter()
    {
        yield return new WaitForSeconds(1);
        fadeIn.SetActive(false);
        wawi.SetActive(true);
        yield return new WaitForSeconds(1);
        mainTextObject.SetActive(true);

        // Recorremos cada línea de storyLines
        foreach (var block in storyBlocks)
        {
            if (block is StoryImage img)
            {
                ImageLoader.ShowImage(img.ImageName);
                yield return new WaitForSeconds(0.3f);
            }
            else if (block is StoryText line)
            {
                // Mostrar personajes solo si no es narrador
                if (line.Speaker.ToLower() == "wawi" || line.Speaker.ToLower() == "wawo")
                {
                    wawi.SetActive(line.Speaker.ToLower() == "wawi");
                    wawo.SetActive(line.Speaker.ToLower() == "wawo");
                    charName.GetComponent<TMPro.TMP_Text>().text = line.Speaker;
                }
                else
                {
                    wawi.SetActive(false);
                    wawo.SetActive(false);
                    charName.GetComponent<TMPro.TMP_Text>().text = "";
                }

                textToSpeak = line.Content;
                textBox.GetComponent<TMPro.TMP_Text>().text = textToSpeak;
                currentTextLength = textToSpeak.Length;
                TextCreator.runTextPrint = true;

            // Espera un frame para que el TMP_Text vuelque el texto completo
                yield return null;
                yield return new WaitForSeconds(0.05f);
                yield return new WaitForSeconds(1);
            // Espera al typewriter
                yield return new WaitUntil(() => textLength == currentTextLength);
            // Pausa antes de la siguiente línea
                yield return new WaitForSeconds(1);
            }
        }


        // Al terminar toda la historia
        nextButton.SetActive(true);
        eventPos = 1;
        Debug.Log("[Scene01] Historia completa.");
    }

    IEnumerator TransitionToQuiz()
    {
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(3);
        Debug.Log("[Scene01] Boton presionado");
    }

    public void NextButton()
    {
        if (eventPos == 1)
        {
            StartCoroutine(TransitionToQuiz());
        }
    }


}
