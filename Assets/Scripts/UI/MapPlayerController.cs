using UnityEngine;
using UnityEngine.UI;

public class MapPlayerController : MonoBehaviour
{
    public float speed = 250f;
    private Image imageRenderer;

    [Header("Panel de Confirmación")]
    public GameObject promptPanel;

    void Start()
    {
        imageRenderer = GetComponent<Image>();

        if (imageRenderer == null)
        {
            Debug.LogError("No se encontró el componente Image en el jugador.");
            return;
        }

        LoadPlayerProfileImage();
    }

    void Update()
    {
        // Si el modal está activo, no permitir movimiento
        if (promptPanel != null && promptPanel.activeSelf)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(h, v, 0) * speed * Time.deltaTime);
    }

    void LoadPlayerProfileImage()
    {
        string imageName = PlayerPrefs.GetString("ProfileImageName", "");

        if (!string.IsNullOrEmpty(imageName))
        {
            Sprite loadedSprite = Resources.Load<Sprite>("Profile/" + imageName);
            if (loadedSprite != null)
            {
                imageRenderer.sprite = loadedSprite;
            }
            else
            {
                Debug.LogWarning("No se pudo cargar la imagen del perfil: " + imageName);
            }
        }
        else
        {
            Debug.LogWarning("No se encontró un nombre de imagen guardado.");
        }
    }
}
