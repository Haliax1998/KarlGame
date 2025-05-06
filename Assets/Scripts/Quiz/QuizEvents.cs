using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class QuizEvents : MonoBehaviour
{
    [Header("Transiciones")]
    [SerializeField] private GameObject fadeIn;

    [Header("UI de Quiz")]
    [Tooltip("Arrastra aquí tu TextMeshProUGUI (antes SpeakText)")]
    [SerializeField] private TextMeshProUGUI questionText;

    // (Opcional) si quieres activar/desactivar todo el panel de pregunta
    [Tooltip("Arrastra aquí el contenedor de la pregunta (por ejemplo QuestionPanel)")]
    [SerializeField] private GameObject questionPanel;


    [Tooltip("Arreglo de 4 GameObjects, cada uno debe tener un componente Button y un TextMeshProUGUI hijo para la etiqueta")]
    [SerializeField] private GameObject[] optionButtonObjects;

    [Tooltip("TextMeshPro para mostrar la pista")]
    [SerializeField] private TextMeshProUGUI hintText;



    // Aquí guardaremos todas las preguntas
    private List<QuestionData> _questions;
    private int _currentIndex;


    void Start()
    {
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

        // 3) Pinta pregunta
        ShowQuestion(_currentIndex);
    }

    /// <summary>
    /// Muestra en pantalla la pregunta número 'index'.
    /// </summary>
    private void ShowQuestion(int index)
    {
        var q = _questions[index];
        questionText.text = $"[{q.topic}] {q.question}";
        hintText.text = "";

        // Para cada GameObject referenciado…
        for (int i = 0; i < optionButtonObjects.Length; i++)
        {
            var btnGO = optionButtonObjects[i];

            // 1) Asigna el texto de la opción
            var label = btnGO.GetComponentInChildren<TextMeshProUGUI>();
            label.text = q.options[i];

            // 2) Obtén el componente Button
            var btn = btnGO.GetComponent<Button>();

            // 3) Limpia listeners y añade el nuevo
            btn.onClick.RemoveAllListeners();
            int capture = i;
            btn.onClick.AddListener(() => OnOptionSelected(capture));

            // 4) Asegúrate de que esté habilitado
            btn.interactable = true;
        }
    }

    private void OnOptionSelected(int chosenIndex)
    {
        var q = _questions[_currentIndex];
        bool correct = chosenIndex == q.correctAnswerIndex;

        hintText.text = correct ? q.legendaryHint : q.epicHint;

        // Desactiva todos los botones para evitar múltiples clicks
        foreach (var go in optionButtonObjects)
            go.GetComponent<Button>().interactable = false;

        // Avanza a la siguiente pregunta tras 1 segundo
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
        questionText.text = "¡Quiz terminado!";
        hintText.text = "";
        foreach (var go in optionButtonObjects)
            go.SetActive(false);
    }

}
