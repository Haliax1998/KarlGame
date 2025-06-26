using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject fadeOut;

    void Start()
    {
        
    }

    public void StartGame()
    {
        StartCoroutine(TransferToAdventureScene());
    }

    public void StartAchievement()
    {
        StartCoroutine(TransferToAchievementScene());
    }

    public void StartCharacter()
    {
        StartCoroutine(TransferToCharacterScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TransferToAdventureScene()
    {
        yield return new WaitForSeconds(0);
        SceneManager.LoadScene(1);
    }

    IEnumerator TransferToCharacterScene()
    {
        yield return new WaitForSeconds(0);
        SceneManager.LoadScene(6);
    }

    IEnumerator TransferToAchievementScene()
    {
        yield return new WaitForSeconds(0);
        SceneManager.LoadScene(5);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
