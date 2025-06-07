using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Burst.CompilerServices;


public class QuizEvents : MonoBehaviour
{
    private int _correctAnswers = 0;


    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button backButton;
    //[SerializeField] private Image medalImage;
    //[SerializeField] private Sprite bronzeMedal;
    //[SerializeField] private Sprite silverMedal;
    //[SerializeField] private Sprite goldMedal;
    [SerializeField] private TextMeshProUGUI resultMessageText;



    [Header("Transiciones")]
    [SerializeField] private GameObject fadeIn;

    private bool usedHint = false;
    private bool usedEliminate = false;
    private bool helpUsedThisTurn = false;

    [SerializeField] private Button hintButton;
    [SerializeField] private Button eliminateButton;



    [Header("UI de Quiz")]
    [Tooltip("Arrastra aquí tu TextMeshProUGUI (antes SpeakText)")]
    [SerializeField] private TextMeshProUGUI questionText;

    [Tooltip("Arrastra aquí el contenedor de la pregunta")]
    [SerializeField] private GameObject questionPanel;


    [Tooltip("Arreglo de 4 GameObjects, cada uno debe tener un componente Button y un TextMeshProUGUI hijo para la etiqueta")]
    [SerializeField] private GameObject[] optionButtonObjects;

    [Tooltip("TextMeshPro para mostrar la pista")]
    [SerializeField] private TextMeshProUGUI markText;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private GameObject hint;


    [Tooltip("TextMeshProUGUI donde se mostrará el puntaje al final")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int _score = 0;
    // Variable que guardará la posición real de la respuesta correcta
    private int _displayedCorrectIndex;


    // Aquí guardaremos todas las preguntas
    private List<QuestionData> _questions;
    private int _currentIndex;


    void Start()
    {
        var music = GameObject.FindObjectOfType<MenuMusic>();
        if (music != null)
        {
            Destroy(music.gameObject);  // Detiene la música del menú
        }

        // 1) Carga y mezcla todas las preguntas
        _questions = QuizLoaderTxtAsXml.LoadAllQuestions()
                       .OrderBy(_ => Random.value)
                       .ToList();
        _currentIndex = 0;

        Debug.Log($"QuizEvents: Cargadas {_questions.Count} preguntas.");

        StartCoroutine(EventStarter());
    }

    IEnumerator EventStarter()
    {
        fadeIn.SetActive(true);
        yield return new WaitForSeconds(2f);
        fadeIn.SetActive(false);

        // 2) Muestra el panel de pregunta (si lo tienes oculto)
        if (questionPanel != null) questionPanel.SetActive(true);

        eliminateButton.gameObject.SetActive(true);
        hintButton.gameObject.SetActive(true);

        // 3) Pinta pregunta
        ShowQuestion(_currentIndex);

    }

    private void UpdateScoreUI()
    {
        if (markText != null)
            markText.text = $"{_score} puntos";
    }


    public void UseHint()
    {
        if (helpUsedThisTurn || usedHint) return;

        hint.SetActive(true);
        hintText.text = "Pista: " + _questions[_currentIndex].legendaryHint;
        _score -= 5;
        usedHint = true;
        helpUsedThisTurn = true;
        UpdateScoreUI();
    }

    public void EliminateTwoOptions()
    {
        if (helpUsedThisTurn || usedEliminate) return;

        int hidden = 0;
        for (int i = 0; i < optionButtonObjects.Length && hidden < 2; i++)
        {
            if (i != _displayedCorrectIndex)
            {
                optionButtonObjects[i].SetActive(false);
                hidden++;
            }
        }

        _score -= 10;
        usedEliminate = true;
        helpUsedThisTurn = true;
        UpdateScoreUI();
    }




    private void ShowQuestion(int index)
    {
        var q = _questions[index];
        questionText.text = $"[{q.topic}] {q.question}";

        var indices = Enumerable.Range(0, q.options.Length)
                                .OrderBy(_ => Random.value)
                                .ToArray();

        var shuffledOptions = indices
            .Select(i => q.options[i])
            .ToArray();

        _displayedCorrectIndex = System.Array.IndexOf(indices, 0);



        // Para cada GameObject referenciado…
        for (int i = 0; i < optionButtonObjects.Length; i++)
        {
            var btnGO = optionButtonObjects[i];

            // 1) Asigna el texto de la opción
            var label = btnGO.GetComponentInChildren<TextMeshProUGUI>();
            label.text = shuffledOptions[i];

            // 2) Obtén el componente Button
            var btn = btnGO.GetComponent<Button>();

            // 3) Limpia listeners y añade el nuevo
            btn.onClick.RemoveAllListeners();
            int capture = i;
            btn.onClick.AddListener(() => OnOptionSelected(capture));

            // 4) Asegúrate de que esté habilitado
            btn.interactable = true;
            label.color = Color.white;
        }

        helpUsedThisTurn = false;
        usedHint = false;
        usedEliminate = false;
        hintText.text = "";
        hint.SetActive(false);

        foreach (var go in optionButtonObjects)
            go.SetActive(true);

    }

    private void OnOptionSelected(int chosenIndex)
    {
        var q = _questions[_currentIndex];
        bool correct = chosenIndex == _displayedCorrectIndex;

        // Sumar 20 puntos si acierta
        if (correct)
        {
            _score += 20;
            _correctAnswers++;
        }
            

        string tag = $"{_score} puntos";
        markText.text = tag;

        Debug.Log(correct ? "¡Correcto!" : "Fallaste…");

        // Pintar botones: correctos en verde, el incorrecto seleccionado en rojo
        for (int i = 0; i < optionButtonObjects.Length; i++)
        {
            var btn = optionButtonObjects[i].GetComponent<Button>();
            var label = optionButtonObjects[i].GetComponentInChildren<TextMeshProUGUI>();

            btn.interactable = false;

            if (i == _displayedCorrectIndex)
                label.color = Color.green; // correcta
            else if (i == chosenIndex)
                label.color = Color.red; // elegida incorrecta
            else
                label.color = Color.black;
        }

        // Avanza tras 1 segundo
        StartCoroutine(NextQuestionAfterDelay(1f));
    }



    private IEnumerator NextQuestionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _currentIndex++;

        if (_currentIndex < _questions.Count)
            ShowQuestion(_currentIndex);
        else
            EndQuiz();
    }

    private void EndQuiz()
    {
        float percentage = ((float)_correctAnswers / _questions.Count) * 100f;
        eliminateButton.gameObject.SetActive(false);
        hintButton.gameObject.SetActive(false);

        foreach (var go in optionButtonObjects)
            go.SetActive(false);

        // Mostrar resultados
        resultPanel.SetActive(true);
        questionText.text = $"¡Quiz terminado!\n{_correctAnswers}/{_questions.Count} ({percentage:F1}%)";

        // Evaluar medalla
        if (percentage >= 90)
        {
            // medalImage.sprite = goldMedal;
            resultMessageText.text = "¡Excelente! Has ganado una medalla de oro.";
        }
        else if (percentage >= 80)
        {
            // medalImage.sprite = silverMedal;
            resultMessageText.text = "¡Buen trabajo! Medalla de plata obtenida.";
        }
        else if (percentage >= 70)
        {
            // medalImage.sprite = bronzeMedal;
            resultMessageText.text = "Aprobaste con una medalla de bronce.";
        }
        else
        {
            // medalImage.enabled = false;
            resultMessageText.text = "Necesitas estudiar más. Puedes intentarlo de nuevo.";
        }

        // Activar botón de reintento si <70%
        retryButton.gameObject.SetActive(percentage < 70);

        // Guardar puntaje C:\Users\PC\AppData\LocalLow\DefaultCompany\
        SaveScoreToFile(percentage);
    }

    private void SaveScoreToFile(float percentage)
    {
        string path = Application.persistentDataPath + "/quiz_scores.txt";
        string storyName = StoryManager.SelectedStory;
        // Formato CSV: Historia;RespuestasCorrectas;TotalPreguntas;Porcentaje;FechaHora
        string line = $"{storyName};{_correctAnswers};{_questions.Count};{percentage:F1};{System.DateTime.Now:yyyy-MM-dd HH:mm:ss}\n";
   
        // Formato humano
        // string line = $"{storyName}: {_correctAnswers}/{_questions.Count} ({percentage:F1}%) - {System.DateTime.Now}\n";
        System.IO.File.AppendAllText(path, line);
    }

    public void RetryQuiz()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void GoToStorySelector()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }



}
