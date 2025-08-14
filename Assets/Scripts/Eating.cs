using UnityEngine;

public class EatFood : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode eatKey = KeyCode.F; 
    public float healAmount = 20f; 
    public float hungerAmount = 20f;

    [Header("References")]
    public PlayerHealth playerStats;
    public PlayerHunger playerHunger;

    private void Update()
    {
    
        if (DropAndPickUpItem.currentHeldItem != null)
        {
            GameObject heldObject = DropAndPickUpItem.currentHeldItem.gameObject;

        
            if (heldObject.CompareTag("Food") && Input.GetKeyDown(eatKey))
            {
                Eat(heldObject);
            }
        }
    }

    private void Eat(GameObject foodObject)
    {
        Debug.Log("Ai mâncat: " + foodObject.name);


        if (playerStats != null)
        {
            playerStats.Heal(healAmount);
        }

        if (playerStats != null)
        {
            playerHunger.ChangeHunger(hungerAmount);
        }


        //dupa ce-l mananca, sa dispara de peste tot
        DropAndPickUpItem.currentHeldItem.PutInInventory(); 
        if (DropAndPickUpItem.currentHeldItem.inventoryManager != null && DropAndPickUpItem.currentHeldItem.itemData != null)
        {
            DropAndPickUpItem.currentHeldItem.inventoryManager.RemoveItemFromSlot(DropAndPickUpItem.currentHeldItem.itemData);
        }

        foodObject.SetActive(false);
        DropAndPickUpItem.currentHeldItem = null;
        DropAndPickUpItem.slotFull = false;
    }

}