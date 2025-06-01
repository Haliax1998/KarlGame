using UnityEngine;
using UnityEngine.UI;

public static class ImageLoader
{
    public static void ShowImage(string imageName)
    {
        if (string.IsNullOrEmpty(imageName)) return;

        var sprite = Resources.Load<Sprite>($"Story/{StoryManager.SelectedStory}/Images/Background/{imageName}");
        var img = GameObject.Find("SceneImage").GetComponent<Image>();

        if (sprite != null)
        {
            img.sprite = sprite;
            img.color = Color.white;
        }
        else
        {
            Debug.LogWarning($"No se encontró la imagen '{imageName}' en Resources/Images/Background/");
        }
    }
}
