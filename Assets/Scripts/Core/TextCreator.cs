using System.Collections;
using TMPro;
using UnityEngine;

public class TextCreator : MonoBehaviour
{
    public static bool runTextPrint;
    public static int charCount;
    [SerializeField] string transferText;

    void Update()
    {
        // Actualizamos la longitud que lee Scene01Events
        charCount = GetComponent<TMP_Text>().text.Length;

        if (runTextPrint)
        {
            runTextPrint = false;
            var viewText = GetComponent<TMP_Text>();
            transferText = viewText.text;
            viewText.text = "";
            StartCoroutine(RollText(viewText));
        }
    }

    IEnumerator RollText(TMP_Text viewText)
    {
        foreach (char c in transferText)
        {
            viewText.text += c;
            yield return new WaitForSeconds(0.03f);
        }
    }
}
