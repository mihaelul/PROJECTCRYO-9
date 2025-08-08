using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlots;

    // Lista itemelor colectate
    public List<Item> inventory = new List<Item>();

    void Start()
    {
        menuActivated = false;
        InventoryMenu.SetActive(false); // Ascundem meniul la start
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.I) && menuActivated)
        {
            InventoryMenu.SetActive(false);
            menuActivated = false;
            foreach (var slot in itemSlots)
            {
                slot.ClearDetails();
            }
        }
        else if (Input.GetKeyDown(KeyCode.I) && !menuActivated)
        {
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }

        // -------- update Sanda --------
        // NU Mai e nevoie am comentat linia cu mouseul din first person
        // ca sa nu mai intre cursorul in joc din cauza camerei actuale, cand meniul e activ ramane mouse-ul ------
        //am pus asta si la dialog sis ebate cap in cap
        //Cursor.lockState = menuActivated ? CursorLockMode.None : CursorLockMode.Locked;
        //Cursor.visible = menuActivated;

    }
    // Update sanda am adaugat parametru gameObject ca sa foloseasca istantele din joc (obiectele puse in scena) si nu unele clonate
    // am incercat si cu istante noi, dar au fost probleme asa ca am ramas pe lucruri simple
    public void AddItem(Item itemData, GameObject itemGO)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (!itemSlots[i].isFull)
            {
                itemSlots[i].AddItem(itemData, itemGO);
                Debug.Log($"Item adaugat: {itemData.itemName}");
                return;
            }
        }

        Debug.Log("Inventarul este plin!");
    }

    // ------------- update Sanda --------
    // facem switch
    public void UseItem(GameObject itemGO)
    {
        // obiect in mana si mana ocupata
        if (DropAndPickUpItem.slotFull && DropAndPickUpItem.currentHeldItem != null)
        {
            // trimitem in inventar obiectul din mana
            DropAndPickUpItem.currentHeldItem.PutInInventory();
        }

        // luam parametri obiectului din inventar
        DropAndPickUpItem itemScript = itemGO.GetComponent<DropAndPickUpItem>();
        if (itemScript != null)
        {
            itemGO.SetActive(true); //activam obiectul din nou ca sa-l vedem in scena

            itemScript.inventoryManager = this;
            itemScript.PickUp(); // apelam functia pentru schimbare

            RemoveItemFromSlot(itemScript.itemData); // il eliminam din inventar
        }
        else
        {
            Debug.LogError("GameObject-ul nu are DropAndPickUpItem.");
        }

    }

    // scoatem item din inventar
    public void RemoveItemFromSlot(Item itemData)
    {
        foreach (var slot in itemSlots)
        {
            // daca gasim itemul dorit il eliminam din slot si il punem in mana
            if (slot.isFull && slot.itemName == itemData.itemName)
            {
                slot.ClearSlot();
                break;
            }
        }
    }
}
