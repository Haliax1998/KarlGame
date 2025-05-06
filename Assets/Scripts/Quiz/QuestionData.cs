[System.Serializable]
public class QuestionData
{
    public string topic;          // Temario o categoría de la pregunta
    public string question;       // Texto de la pregunta
    public string[] options;      // Opciones de respuesta (a1..a4)
    public int correctAnswerIndex; // Índice de la respuesta correcta en el array (siempre 0 tras cargar)
    public string epicHint;       // Pista tipo épica
    public string legendaryHint;  // Pista tipo legendaria
}