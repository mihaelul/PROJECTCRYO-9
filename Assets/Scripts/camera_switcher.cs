using UnityEngine;

public class CameraSwitchMenu : MonoBehaviour
{
    public Camera mainCamera;       // camera gameplay
    public Camera menuCamera;       // camera pentru meniu
    public GameObject menuUI;       // UI cu 4 butoane

    private bool playerInRange = false;

    void Start()
    {
        menuUI.SetActive(false);
        menuCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenMenu();
        }
    }

    void OpenMenu()
    {
        mainCamera.gameObject.SetActive(false);
        menuCamera.gameObject.SetActive(true);
        menuUI.SetActive(true);
        // Dacă ai control player, aici îl poți dezactiva
    }

    public void CloseMenu()
    {
        menuUI.SetActive(false);
        menuCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        // Reactivare control player dacă l-ai oprit
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
