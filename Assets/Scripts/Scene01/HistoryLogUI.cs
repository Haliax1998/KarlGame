using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HistoryLogUI : MonoBehaviour
{
    public Text logText;

    void OnEnable()
    {
        var entries = HistoryLog.GetEntries();
        StringBuilder sb = new StringBuilder();

        foreach (var entry in entries)
        {
            sb.AppendLine($"<b>[{entry.Speaker}]</b>: {entry.Content}");
        }

        logText.text = sb.ToString();
        StartCoroutine(ScrollToBottomNextFrame());
    }

    IEnumerator ScrollToBottomNextFrame()
    {
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();

        ScrollRect scroll = GetComponentInChildren<ScrollRect>();
        yield return null; // otro frame más por seguridad
        scroll.verticalNormalizedPosition = 0f;
    }
}
