using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCreationManager : MonoBehaviour
{
    [Header("Inputs")]
    public TMP_InputField nameInput;

    [Header("Contenedor de botones (parent con los 9 botones hijos)")]
    public Transform characterButtonContainer;

    [Header("Botón de guardar")]
    public Button saveButton;

    private Button[] characterButtons;
    private string[] spriteNames;
    private int selectedIndex = -1;

    void Start()
    {
        // Carga los sprites desde Resources/Profile/
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Profile");
        spriteNames = new string[loadedSprites.Length];

        // Obtén los botones hijos del contenedor
        characterButtons = characterButtonContainer.GetComponentsInChildren<Button>();

        // Asigna sprites a los botones automáticamente
        for (int i = 0; i < characterButtons.Length; i++)
        {
            if (i < loadedSprites.Length)
            {
                Image img = characterButtons[i].GetComponent<Image>();
                img.sprite = loadedSprites[i];
                spriteNames[i] = loadedSprites[i].name;

                int index = i; // necesario para el closure
                characterButtons[i].onClick.AddListener(() => SelectCharacter(index));
            }
            else
            {
                // Desactiva botones si hay menos sprites
                characterButtons[i].gameObject.SetActive(false);
            }
        }

        saveButton.onClick.AddListener(SaveCharacter);
    }

    void SelectCharacter(int index)
    {
        selectedIndex = index;

        // Reinicia los colores
        foreach (var btn in characterButtons)
        {
            btn.GetComponent<Image>().color = Color.white;
        }

        // Marca el seleccionado
        characterButtons[index].GetComponent<Image>().color = Color.green;
    }

    void SaveCharacter()
    {
        string playerName = nameInput.text;

        if (string.IsNullOrEmpty(playerName) || selectedIndex == -1)
        {
            Debug.LogWarning("Falta nombre o imagen.");
            return;
        }

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetInt("ProfileIndex", selectedIndex);
        PlayerPrefs.SetString("ProfileImageName", spriteNames[selectedIndex]);
        PlayerPrefs.Save();

        Debug.Log($"Guardado: {playerName}, Index: {selectedIndex}, Imagen: {spriteNames[selectedIndex]}");

        GoBack();
    }

    public void GoBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


}
