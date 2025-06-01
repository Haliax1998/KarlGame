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
        fadeOut.SetActive(true);
        StartCoroutine(TransferToAdventureScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TransferToAdventureScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }
}
