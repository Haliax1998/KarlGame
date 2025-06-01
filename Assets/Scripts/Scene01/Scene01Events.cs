using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene01Events : MonoBehaviour
{

    [SerializeField] public UnityEngine.UI.Image portraitImage;
    public GameObject textBox;
    AudioSource backgroundAudio;
    AudioSource voiceAudio;
    private bool isLogOpen = false;
    private bool clickedInArea = false;

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
        backgroundAudio = GameObject.Find("BackgroundAudio").GetComponent<AudioSource>();
        voiceAudio = GameObject.Find("VoiceAudio").GetComponent<AudioSource>();
        StartCoroutine(EventStarter());

    }


    public void RegisterClick()
    {
        if (!isLogOpen)
        {
            clickedInArea = true;
        }
    }
    IEnumerator EventStarter()
    {
        yield return new WaitForSeconds(1);
        fadeIn.SetActive(false);
        yield return new WaitForSeconds(1);
        mainTextObject.SetActive(true);
        portraitImage.color = new Color(1, 1, 1, 0);

        // Recorremos cada línea de storyLines
        foreach (var block in storyBlocks)
        {
            if (block is StoryImage img)
            {
                ImageLoader.ShowImage(img.ImageName);
                backgroundAudio.Stop();
                if (!string.IsNullOrEmpty(img.AudioName))
                {
                    backgroundAudio.loop = true;
                    var bgClip = Resources.Load<AudioClip>($"Story/{StoryManager.SelectedStory}/Audio/{img.AudioName}");
                    if (bgClip != null)
                    {
                        backgroundAudio.clip = bgClip;
                        backgroundAudio.Play();
                    }
                }
                yield return new WaitForSeconds(0.3f);
            }
            else if (block is StoryText line)
            {
                voiceAudio.Stop();
                // Mostrar personajes solo si no es narrador
                if (line.Speaker.ToLower() == "narrador")
                {
                    portraitImage.color = new Color(1, 1, 1, 0); // ocultar
                    charName.GetComponent<TMPro.TMP_Text>().text = "Narrador";
                }
                else
                {
                    var sprite = Resources.Load<Sprite>($"Story/{StoryManager.SelectedStory}/Images/Characters/{line.Speaker.ToLower()}");
                    if (sprite != null)
                    {
                        portraitImage.sprite = sprite;
                        portraitImage.color = Color.white;
                    }
                    else
                    {
                        portraitImage.color = new Color(1, 1, 1, 0); // ocultar si no se encuentra
                    }

                    charName.GetComponent<TMPro.TMP_Text>().text = line.Speaker;
                }

                textToSpeak = line.Content;
                HistoryLog.AddEntry(line.Speaker, line.Content, line.AudioName);

                if (!string.IsNullOrEmpty(line.AudioName))
                {
                    voiceAudio.Stop();
                    voiceAudio.loop = false;
                    var voiceClip = Resources.Load<AudioClip>($"Story/{StoryManager.SelectedStory}/Audio/{line.AudioName}");
                    if (voiceClip != null)
                    {
                        voiceAudio.clip = voiceClip;
                        voiceAudio.Play();
                    }
                }
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
                clickedInArea = false;
                yield return new WaitUntil(() => clickedInArea);
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
        Debug.Log($"Botón presionado. eventPos: {eventPos}");
        if (eventPos == 1)
        {
            Debug.Log("Iniciando transición a la escena...");
            StartCoroutine(TransitionToQuiz());
        }
    }

    public void SetLogOpen(bool state)
    {
        isLogOpen = state;
    }

}
