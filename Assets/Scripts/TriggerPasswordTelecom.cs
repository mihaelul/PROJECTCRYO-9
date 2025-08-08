using UnityEngine;

public class TriggerPasswordTelecom : MonoBehaviour
{
    public GameObject passwordPanel; // referinta catre UI-ul de parola
    private bool menuActivated = false;


    // NU Mai e nevoie am comentat linia cu mouseul din first person
    //void LateUpdate()
    //{
    //    // FORtEAZa cursorul dupa ce toate celelalte scripturi au rulat Update()
    //    //if (menuActivated)
    //    //{
    //    //    Cursor.lockState = CursorLockMode.None;
    //    //    Cursor.visible = true;
    //    //}
    //    //else
    //    //{
    //    //    Cursor.lockState = CursorLockMode.Locked;
    //    //    Cursor.visible = false;
    //    //}
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerCam>() != null)
        {
            passwordPanel.SetActive(true);
            menuActivated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerCam>() != null)
        {
            passwordPanel.SetActive(false);
            menuActivated = false;
        }
    }
}