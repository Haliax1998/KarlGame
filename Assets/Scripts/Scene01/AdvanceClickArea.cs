using UnityEngine;
using UnityEngine.EventSystems;

public class AdvanceClickArea : MonoBehaviour, IPointerDownHandler
{
    public Scene01Events sceneController;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (sceneController != null)
        {
            sceneController.RegisterClick();
        }
    }
}
