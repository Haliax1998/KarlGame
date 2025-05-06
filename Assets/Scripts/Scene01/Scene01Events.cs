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

    List<StoryLineData> storyLines;

    void Update()
    {
        textLength = TextCreator.charCount;
    }

    void Start()
    {
        // Carga todas las líneas de la historia
        storyLines = ScriptLoaderTxtAsXml.LoadStoryLines();
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
        foreach (var line in storyLines)
        {
            // Asigna quién habla
            wawi.SetActive(line.Speaker == "wawi");
            wawo.SetActive(line.Speaker == "wawo");

            charName.GetComponent<TMPro.TMP_Text>().text = line.Speaker;

            // Prepara el texto
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


        // Al terminar toda la historia
        //mainTextObject.SetActive(false);
        //wawi.SetActive(false);
        //wawo.SetActive(false);
        nextButton.SetActive(true);
        eventPos = 1;
        Debug.Log("[Scene01] Historia completa.");
    }

    IEnumerator EventOne()
    {
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
        Debug.Log("[Scene01] Boton presionado");
    }

    public void NextButton()
    {
        if (eventPos == 1)
        {
            StartCoroutine(EventOne());
        }
    }


}
