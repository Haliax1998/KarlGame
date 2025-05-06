using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizEvents : MonoBehaviour
{

    [SerializeField] GameObject fadeIn;
    void Start()
    {
        StartCoroutine(EventStarter());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator EventStarter()
    {
        yield return new WaitForSeconds(2);
        fadeIn.SetActive(true);
        yield return new WaitForSeconds(2);
        fadeIn.SetActive(false);
    }

}
