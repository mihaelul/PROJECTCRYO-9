using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class task_manager : MonoBehaviour
{
    // Alex - folosesc un contorul asta in ItemPlacement_O2.cs ca sa pot verifica 
    // daca task-ul a fost terminat
    [Header("O2 Variables")]
    public int neededTanks;

    public GameObject textPrefabTMP;         // Prefab cu TextMeshProUGUI
    public Transform taskListContainer;      // Panel-ul cu VerticalLayout

    [Header("Popup elements")]

    public GameObject O2_SuccessPopup;
    public string popupTaskDescription;
    public Image popupCrossout;
    public float delay = 5f;                     // cat timp popup-ul e activ
    private class Task
    {
        public string descriere;
        public bool completat;
        public TextMeshProUGUI taskUI;

        public Task(string descriere, bool completat = false)
        {
            this.descriere = descriere;
            this.completat = completat;
        }
    }

    private List<Task> taskList = new List<Task>();

    void Start()
    {
        taskList.Add(new Task("Repara sistemul de oxigen din laborator."));
        taskList.Add(new Task("Repara sistemul de filtrare a apei din laborator."));
        taskList.Add(new Task("Gaseste o masca pentru a completa exchipamentul de exterior."));
        taskList.Add(new Task("Gaseste hrana."));
        taskList.Add(new Task("Gaseste adresa bogatasului."));
        taskList.Add(new Task("Gaseste cheia pentru a intra in casa bogatasului."));
        taskList.Add(new Task("Gaseste un cod pentru a realiza comunicarea cu societatea de pe Marte."));
        taskList.Add(new Task("Repara antena."));
        taskList.Add(new Task("Recupereaza codul de acces al camerei cu plante de la dr. Frank."));
        taskList.Add(new Task("Incarca plantele in racheta."));
        taskList.Add(new Task("Decoleaza spre Marte."));

        // marcheaza task-urile la care s-au luat prima data obiectele inainte sa se deschida prima data meniul cu misiuni
        if (GasMaskUi.instance != null && GasMaskUi.instance.completMision > 0)
        {
            CompleteTaskByKeyword("Gaseste o masca pentru a completa exchipamentul de exterior.");
        }

        if (FoodMission.instance != null && FoodMission.instance.contor > 0)
        {
            CompleteTaskByKeyword("Gaseste hrana.");
        }

        SetupPopups();
        RenderTask();
    }

    void Update()
    {
        // Debug.Log($"neededTanks current value is {neededTanks}");
        if (neededTanks == 0)
        {
            foreach (var task in taskList)
            {
                if (task.descriere == "Repara sistemul de oxigen din laborator.")
                {
                    if (task.completat != true)
                    {
                        task.completat = true;
                        UpdateTask(task);   // Update UI
                    }
                    break;
                }
            }
        }

        // marcheaza task-urile daca s-a deschis meniul prima data si pe urma au fost luate obiectele
        if (GasMaskUi.instance != null && GasMaskUi.instance.completMision > 0)
        {
            CompleteTaskByKeyword("Gaseste o masca pentru a completa exchipamentul de exterior.");
        }

        if (FoodMission.instance != null && FoodMission.instance.contor > 0)
        {
            CompleteTaskByKeyword("Gaseste hrana.");
        }
    }

    // completare task general in functie de enunt
    public void CompleteTaskByKeyword(string keyword)
    {
        foreach (var task in taskList)
        {
            if (task.descriere.Contains(keyword))
            {
                if (!task.completat)
                {
                    task.completat = true;
                    UpdateTaskGeneral(task);
                }
                return; // oprim la primul gasit
            }
        }
    }

    // taiere task pentru oricare general
    void UpdateTaskGeneral(Task task)
    {
        if (task.taskUI != null)
        {
            // string simbol = task.completat ? "☑" : "□";
            task.taskUI.text = $"{task.descriere}";

            // Optional visual enhancements
            task.taskUI.fontStyle = task.completat ? FontStyles.Strikethrough : FontStyles.Normal;
            task.taskUI.color = task.completat ? new Color(0.7f, 0.7f, 0.7f) : Color.white;

        }
    }

    public void RenderTask()
    {
        foreach (Transform child in taskListContainer)
            Destroy(child.gameObject); // curăță vechile taskuri

        foreach (var task in taskList)
        {
            GameObject taskTextGO = Instantiate(textPrefabTMP, taskListContainer);
            TextMeshProUGUI tmpText = taskTextGO.GetComponent<TextMeshProUGUI>();

            task.taskUI = tmpText;

            // string simbol = task.completat ? "☑" : "□";
            tmpText.text = $"{task.descriere}";
            tmpText.fontStyle = task.completat ? FontStyles.Strikethrough : FontStyles.Normal;
            tmpText.color = task.completat ? new Color(0.7f, 0.7f, 0.7f) : Color.white;
        }
    }

    void UpdateTask(Task task)
    {
        if (task.taskUI != null)
        {
            // string simbol = task.completat ? "☑" : "□";
            task.taskUI.text = $"{task.descriere}";

            // Optional visual enhancements
            task.taskUI.fontStyle = task.completat ? FontStyles.Strikethrough : FontStyles.Normal;
            task.taskUI.color = task.completat ? new Color(0.7f, 0.7f, 0.7f) : Color.white;

            ShowTaskCompletePopup(task);
        }
    }

    void SetupPopups()
    {
        O2_SuccessPopup.SetActive(false);
    }

    void ShowTaskCompletePopup(Task task)
    {
        popupTaskDescription = task.descriere;
        O2_SuccessPopup.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(AutoHidePopup(delay));
    }
    IEnumerator AutoHidePopup(float delay)
    {
        yield return new WaitForSeconds(delay);    // wait 5 seconds
        O2_SuccessPopup.SetActive(false);
    }
}
