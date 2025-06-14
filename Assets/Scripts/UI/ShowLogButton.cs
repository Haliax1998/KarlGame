using UnityEngine;
using UnityEngine.UI;

public class ShowLogButton : MonoBehaviour
{
    [Header("Log Controls")]
    [SerializeField] public GameObject logPanel;
    [SerializeField] Scene01Events sceneController;
    [SerializeField] public GameObject logButton;

    [Header("Text Panel Controls")]
    [SerializeField] public GameObject textPanel;
    [SerializeField] public Button toggleTextButton;
    [SerializeField] public Sprite eyeOpenSprite;
    [SerializeField] public Sprite eyeClosedSprite;

    [SerializeField] private CanvasGroup textPanelCanvasGroup;


    private bool isTextVisible = true;

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


    public void ToggleTextVisibility()
    {
        isTextVisible = !isTextVisible;

        if (textPanelCanvasGroup != null)
        {
            textPanelCanvasGroup.alpha = isTextVisible ? 1 : 0;
            textPanelCanvasGroup.interactable = isTextVisible;
            textPanelCanvasGroup.blocksRaycasts = isTextVisible;
        }

        if (toggleTextButton != null && toggleTextButton.image != null)
        {
            toggleTextButton.image.sprite = isTextVisible ? eyeOpenSprite : eyeClosedSprite;
        }
    }

}
