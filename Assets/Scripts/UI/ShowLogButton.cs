using UnityEngine;

public class ShowLogButton : MonoBehaviour
{
    [SerializeField] public GameObject logPanel;
    [SerializeField] Scene01Events sceneController;
    [SerializeField] public GameObject logButton;

    public void ShowLog()
    {
        if (logPanel != null)
        {
            logPanel.SetActive(true);
            sceneController.SetLogOpen(true);
            logButton.SetActive(false);

        }
    }

    public void HideLog()
    {
        if (logPanel != null)
        {
            logPanel.SetActive(false);
            sceneController.SetLogOpen(false);
            logButton.SetActive(true);
        }
    }
}
