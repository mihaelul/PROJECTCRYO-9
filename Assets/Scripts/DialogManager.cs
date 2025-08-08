using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{

    public Image avatarImage;
    public Text actorName;
    public Text dialogText;
    public RectTransform backgroundTranform;
    public GameObject DialogPanel;

    Message[] curentMasages;
    Actor[] curentActors;
    int activeMassage = 0;
    public static bool isActive = false;

    public void OpenDialogue(Message[] mesaje, Actor[] actors)
    {
        curentActors = actors;
        curentMasages = mesaje;
        activeMassage = 0;
        isActive = true;

        Debug.Log("Start messages.Loaded message " + mesaje.Length);
        DisplayMessage();
    }

    public void DisplayMessage()
    {
        Message messageToDisplay = curentMasages[activeMassage];
        dialogText.text = messageToDisplay.message;
        Actor actorToDisplay = curentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        avatarImage.sprite = actorToDisplay.sprite;
    }

    public void NextMessage()
    {
        activeMassage++;
        // nu iese din numaul de replici
        if (activeMassage < curentMasages.Length) { 
            DisplayMessage();
        }else
        {
            Debug.Log("Conversatie terminata!");
            DialogPanel.SetActive(false);
            isActive = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // pentru urmatoare replica din dialog o sa se apese N, se poate si cu space chiar daca sare caracterul in scriptul de movement al caracterul la update punem
        // if(DialogManager.isActive == true ) return;
        if (Input.GetKeyDown(KeyCode.Space) && isActive) { 
            NextMessage();
        }
        
    }
}
