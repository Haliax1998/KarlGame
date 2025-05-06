[System.Serializable]
public class QuestionData
{
    public string topic;          // Temario o categor�a de la pregunta
    public string question;       // Texto de la pregunta
    public string[] options;      // Opciones de respuesta (a1..a4)
    public int correctAnswerIndex; // �ndice de la respuesta correcta en el array (siempre 0 tras cargar)
    public string epicHint;       // Pista tipo �pica
    public string legendaryHint;  // Pista tipo legendaria
}