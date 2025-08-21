using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // cursorul ramane mereu vizibil
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
