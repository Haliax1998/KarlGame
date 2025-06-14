using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StorySelector : MonoBehaviour
{
    [SerializeField] private GameObject fadeOut;
    [SerializeField] private float delay = 3f;
    [SerializeField] private string targetScene = "AdventureScene";

    public void SelectStory(string storyName)
    {
        StoryManager.SelectedStory = storyName;
        Debug.Log("Historia seleccionada: " + StoryManager.SelectedStory);
        StartCoroutine(TransferToAdventureScene());
    }

    IEnumerator TransferToAdventureScene()
    {
        if (fadeOut != null)
        {
            fadeOut.SetActive(true);
        }
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(targetScene);
    }

    public void GoToStorySelector()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
