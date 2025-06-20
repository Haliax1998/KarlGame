using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    [System.Serializable]
    public class StoryUI
    {
        public string storyName;
        public TextMeshProUGUI storyText;
        public Image medalImage;
        public TextMeshProUGUI scoreText;
    }

    [Header("Lista de logros por historia")]
    public List<StoryUI> stories;

    [Header("Sprites de medallas")]
    public Sprite bronzeMedal;
    public Sprite silverMedal;
    public Sprite goldMedal;
    public Sprite noMedal;

    void Start()
    {
        LoadAchievements();
    }

    void LoadAchievements()
    {
        string path = Application.persistentDataPath + "/quiz_scores.txt";

        if (!File.Exists(path))
        {
            Debug.LogWarning("No se encontró el archivo de puntajes. Mostrando logros vacíos.");
            // Muestra "0%" y noMedal para cada historia
            foreach (var storyUI in stories)
            {
                storyUI.scoreText.text = "0%";
                storyUI.medalImage.sprite = noMedal;
            }
            return;
        }

        var lines = File.ReadAllLines(path);

        foreach (var storyUI in stories)
        {
            float bestPercentage = 0f;

            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts.Length >= 5 && parts[0] == storyUI.storyName)
                {
                    if (float.TryParse(parts[3], out float percent))
                    {
                        bestPercentage = Mathf.Max(bestPercentage, percent);
                    }
                }
            }

            // Actualiza UI
            storyUI.storyText.text = storyUI.storyName;
            storyUI.scoreText.text = $"{bestPercentage:F1}%";

            // Asigna medalla
            if (bestPercentage >= 90)
                storyUI.medalImage.sprite = goldMedal;
            else if (bestPercentage >= 80)
                storyUI.medalImage.sprite = silverMedal;
            else if (bestPercentage >= 70)
                storyUI.medalImage.sprite = bronzeMedal;
            else
                storyUI.medalImage.sprite = noMedal;
        }
    }


    public void GoToStorySelector()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
