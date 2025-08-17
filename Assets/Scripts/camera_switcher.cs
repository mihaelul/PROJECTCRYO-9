using UnityEngine;

public class CanvasSwitchMenu : MonoBehaviour
{
    public GameObject gameplayCanvas; // HUD sau canvas-ul activ in gameplay
    public GameObject menuCanvas;     // Canvas-ul cu butoane pentru meniu
    public MonoBehaviour playerController; // Scriptul care controlează player-ul

    private bool playerInRange = false;
    private bool menuOpen = false;

    void Start()
    {
        menuCanvas.SetActive(false);
        gameplayCanvas.SetActive(true);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!menuOpen)
                OpenMenu();
            else
                CloseMenu();
        }
    }

    void OpenMenu()
    {
        gameplayCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        menuOpen = true;

        // Dezactivează controlul playerului
        if (playerController != null) 
            playerController.enabled = false;

        // Arată cursorul și permite interacțiunea cu UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMenu()
    {
        menuCanvas.SetActive(false);
        gameplayCanvas.SetActive(true);
        menuOpen = false;

        // Reactivează controlul playerului
        if (playerController != null)
            playerController.enabled = true;

        // Ascunde cursorul
        // Nu mai ascudem cursorul ca trebuie sa l folosim sa interactionam cu UI
        // daca il ascunzi dupa ce termini cu meniul celelalte nu o sa functioneze - Sanda
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
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
