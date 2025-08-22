using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    public GameObject panel;    // background_desktop - pt monitor
    public GameObject InventoryMenu;
    public GameObject StartMenu;
    public GameObject StartMenuInfo;
    private PlayerCam PlayerCam;   // camera rotation specifically

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        GameObject PlayerModel = GameObject.Find("PlayerModel");
        PlayerCam = PlayerModel.GetComponent<PlayerCam>();
    }

    void Update()
    {
        float copyXRotation = PlayerCam.xRotation;
        float copyYRotation = PlayerCam.yRotation;

        Debug.Log("panel = " + panel.activeSelf + "\nInventoryMenu = " + InventoryMenu.activeSelf + "\nStartMenu = " + StartMenu.activeSelf + "\nStartMenuInfo = " + StartMenuInfo.activeSelf);
        // cursorul ramane mereu vizibil
        if (panel.activeSelf == true || InventoryMenu.activeSelf == true || StartMenu.activeSelf == true || StartMenuInfo.activeSelf == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (InventoryMenu.activeSelf == true)
        {
            PlayerCam.lookAction.Disable();
        }
        else
        {
            PlayerCam.lookAction.Enable();
        }
    }
}
