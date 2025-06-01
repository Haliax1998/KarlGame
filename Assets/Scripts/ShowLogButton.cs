using UnityEngine;

public class ShowLogButton : MonoBehaviour
{
    [SerializeField] public GameObject logPanel;
    [SerializeField] Scene01Events sceneController;

    public void ShowLog()
    {
        if (logPanel != null)
        {
            logPanel.SetActive(true);
            sceneController.SetLogOpen(true);
        }
    }

    public void HideLog()
    {
        if (logPanel != null)
        {
            logPanel.SetActive(false);
            sceneController.SetLogOpen(false);
        }
    }
}
