using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Camera playerCamera;       // camera care urmează jucătorul
    public Camera terminalCamera;     // camera fixă spre calculator
    public GameObject terminalUI;     // canvas-ul meniului
    public GameObject playerController; // player-ul sau controllerul său

    private bool nearTerminal = false;
    private bool inTerminalView = false;

    void Update()
    {
        if (nearTerminal && !inTerminalView && Input.GetKeyDown(KeyCode.E))
        {
            EnterTerminalView();
        }

        if (inTerminalView && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitTerminalView();
        }
    }

    void EnterTerminalView()
    {
        inTerminalView = true;

        playerCamera.enabled = false;
        terminalCamera.enabled = true;

        terminalUI.SetActive(true);
        playerController.SetActive(false); // oprește mișcarea
    }

    public void ExitTerminalView()
    {
        inTerminalView = false;

        playerCamera.enabled = true;
        terminalCamera.enabled = false;

        terminalUI.SetActive(false);
        playerController.SetActive(true); // repornește mișcarea
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nearTerminal = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nearTerminal = false;
        }
    }
}
