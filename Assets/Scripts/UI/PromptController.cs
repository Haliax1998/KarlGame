using UnityEngine;

public class PromptController : MonoBehaviour
{
    private string storyName;

    public void Prepare(string name)
    {
        storyName = name;
        gameObject.SetActive(true);
    }

    public void Confirm()
    {
        Debug.Log("Confirmación: historia a enviar -> " + storyName);
        FindObjectOfType<StorySelector>().SelectStory(storyName);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }
}
