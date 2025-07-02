using UnityEngine;

public class StoryTriggerZone : MonoBehaviour
{
    public string storyName;
    public GameObject promptPanel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hola, entré al trigger");
            Debug.Log("Nombre de historia seleccionada: " + storyName);
            promptPanel.GetComponent<PromptController>().Prepare(storyName);
        }
    }
}
