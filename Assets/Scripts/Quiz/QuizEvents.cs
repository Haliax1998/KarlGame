using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;  // ←

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

    // Aquí guardaremos todas las preguntas
    private List<QuestionData> _questions;

    void Start()
    {
        // 1) Carga y mezcla todas las preguntas
        _questions = QuizLoaderTxtAsXml.LoadAllQuestions()
                       .OrderBy(_ => Random.value)
                       .ToList();

        Debug.Log($"QuizEvents: Cargadas {_questions.Count} preguntas.");

        StartCoroutine(EventStarter());
    }

    IEnumerator EventStarter()
    {
        yield return new WaitForSeconds(2f);
        fadeIn.SetActive(true);
        yield return new WaitForSeconds(2f);
        fadeIn.SetActive(false);

        // 2) Muestra el panel de pregunta (si lo tienes oculto)
        if (questionPanel != null) questionPanel.SetActive(true);

        // 3) Pinta la primera pregunta
        ShowQuestion(0);
    }

    /// <summary>
    /// Muestra en pantalla la pregunta número 'index'.
    /// </summary>
    private void ShowQuestion(int index)
    {
        if (_questions == null || _questions.Count == 0)
        {
            questionText.text = "¡No hay preguntas disponibles!";
            return;
        }

        // Protege del índice fuera de rango
        index = Mathf.Clamp(index, 0, _questions.Count - 1);

        var q = _questions[index];
        // Formato: [Tema] Pregunta
        questionText.text = $"[{q.topic}] {q.question}";
    }
}
