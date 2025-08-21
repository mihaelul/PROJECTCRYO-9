using NUnit.Framework.Interfaces;
using UnityEngine;

public class FoodMission : MonoBehaviour
{
    public static FoodMission instance;
    public task_manager taskManager;
    public int contor = 0;
    private bool collected = false;

    void Awake()
    {
        instance = this;
    }

    // Apelata cand jucatorul ia obiectul
    public void OnFoodCollected(DropAndPickUpItem obj)
    {
        if (collected) return; // prevenim numararea dubla

        if (obj.itemData != null)
        {
            switch (obj.itemData.itemName)
            {
                case "Mar1":
                case "Mar2":
                case "Mar3":
                    collected = true;
                    contor++;
                    break;
            }
        }
    }
}
