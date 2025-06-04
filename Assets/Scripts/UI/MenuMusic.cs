using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    private static MenuMusic instance;

    void Awake()
    {
        // Si ya hay una instancia, destruir esta nueva
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Marcar como instancia �nica y no destruir
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
