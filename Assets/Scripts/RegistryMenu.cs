using UnityEngine;

[RequireComponent(typeof(DropAndPickUpItem))]
public class ShowRegistryOnE : MonoBehaviour
{
    public GameObject panelRoot; // drag: RegistryInfo

    DropAndPickUpItem pickup;

    void Awake()
    {
        pickup = GetComponent<DropAndPickUpItem>();
        if (panelRoot) panelRoot.SetActive(false);
    }

    void Update()
    {
        if (!panelRoot) return;

        bool isHeld = (DropAndPickUpItem.currentHeldItem == pickup);


        if (!panelRoot.activeSelf && isHeld && ManagerItem.pickedByE == pickup) {
            Open();
            ManagerItem.pickedByE = null;
        }

        // ESC inchide
        if (panelRoot.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            Close();

        if (panelRoot.activeSelf && !isHeld)
            Close();
    }

    void Open() {
        panelRoot.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Close() {
        panelRoot.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
