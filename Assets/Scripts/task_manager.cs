using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class task_manager : MonoBehaviour
{
    public GameObject textPrefabTMP;         // Prefab cu TextMeshProUGUI
    public Transform taskListContainer;      // Panel-ul cu VerticalLayout

    private class Task
    {
        public string descriere;
        public bool completat;

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
        taskList.Add(new Task("Repara sistemul de apa din laborator."));
        taskList.Add(new Task("Gaseste o masca pentru a completa exchipamentul de exterior."));
        taskList.Add(new Task("Gaseste hrana."));
        taskList.Add(new Task("Gaseste adresa bogatasului."));
        taskList.Add(new Task("Gaseste cheia pentru a intra in casa bogatasului."));
        taskList.Add(new Task("Gaseste un cod pentru a realiza comunicarea cu societatea de pe Marte."));
        taskList.Add(new Task("Repara antena."));
        taskList.Add(new Task("Recupereaza codul de acces al camerei cu plante de la dr. Frank."));
        taskList.Add(new Task("Repara sistemul de oxigen din laborator."));
        taskList.Add(new Task("Incarca plantele in racheta."));
        taskList.Add(new Task("Decoleaza spre Marte."));

        RenderTask();
    }

    void RenderTask()
    {
        foreach (Transform child in taskListContainer)
            Destroy(child.gameObject); // curăță vechile taskuri

        foreach (var task in taskList)
        {
            GameObject taskTextGO = Instantiate(textPrefabTMP, taskListContainer);
            TextMeshProUGUI tmpText = taskTextGO.GetComponent<TextMeshProUGUI>();

            string simbol = task.completat ? "☑" : "□";
            tmpText.text = $"{simbol} {task.descriere}";
        }
    }
}
