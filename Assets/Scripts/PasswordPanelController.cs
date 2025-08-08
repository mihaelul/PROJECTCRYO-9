using UnityEngine;
using UnityEngine.UI;

public class PasswordPanelController : MonoBehaviour
{
    public InputField passwordInput;
    public string correctPassword = "1234"; // parola corecta
    public GameObject passwordPanel;

    // dialog trigger
    public GameObject DialogPanel;
    public Message[] messages;
    public Actor[] actors;


    public void CheckPassword()
    {
        if (passwordInput.text == correctPassword)
        {
            Debug.Log("Parola corecta!");
            passwordPanel.SetActive(false); // inchide panel-ul activez pe urma panelul cu conversatia
            DialogPanel.SetActive(true); // activare panel cu dialogul de la atena de telcomunicatii
            FindAnyObjectByType<DialogManager>().OpenDialogue(messages, actors); // star dialog
            // Aici poti apela o functie pentru a deschide usa sau altceva
            
        }
        else
        {
            Debug.Log("Parola gresita!");
            passwordInput.text = ""; // goleste campul
            
        }
    }
}

// clase pentru dialog

[System.Serializable]
public class Message
{
    public int actorId;
    public string message;
}

[System.Serializable]
public class Actor
{
    public string name;
    public Sprite sprite;
}