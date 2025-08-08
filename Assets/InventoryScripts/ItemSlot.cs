using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemSlot : MonoBehaviour
{
    // datele obiectului
    public string itemName;
    public int quantity = 1;
    public Sprite itemSprite;
    public bool isFull = false;
    public string itemDescription;

    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    // descriere
    [SerializeField] private Image itemDetailImage;
    [SerializeField] private TMP_Text itemDetailDescription;
    [SerializeField] private TMP_Text itemDescriptionName; 

    // ------ update Sanda --------
    public InventoryManager inventoryManager; // setat din inspector
    public GameObject itemObject; // referinta la obiectul fizic din scena

    // am mai pus un parametru pt obiect - Sanda
    public void AddItem(Item itemData, GameObject itemGO)
    {
        itemName = itemData.itemName;
        quantity = 1;
        itemSprite = itemData.icon;
        isFull = true;
        itemDescription = itemData.description;

        // update sanda 
        itemObject = itemGO; // intanta obiect din scena

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
        itemImage.sprite = itemSprite;
        itemImage.enabled = true;
    }

    public void OnClick()
    {
        if (!isFull) return;

        // Actualizam descrierea
        if (itemDetailImage != null && itemDetailDescription != null)
        {
            itemDetailImage.sprite = itemSprite;
            itemDetailImage.enabled = true;

            Item itemData = itemObject.GetComponent<DropAndPickUpItem>().itemData;
            if (itemData != null)
            {
                itemDetailDescription.text = itemData.description;
            }
            else
            {
                itemDetailDescription.text = "Descriere indisponibilă.";
            }
            itemDescriptionName.text = itemData.itemName;
        }

        Debug.Log("Clicked on slot: " + itemName);
        inventoryManager.UseItem(itemObject); // trimitem obiectul
    }


    // golire slot din inventar
    public void ClearSlot()
    {
        itemName = "";
        quantity = 0;
        itemSprite = null;
        isFull = false;
        itemObject = null;

        quantityText.enabled = false;
        itemImage.enabled = false;
    }
    
       //golire descriere
    public void ClearDetails()
{
    if (itemDetailImage != null)
        itemDetailImage.enabled = false;

    if (itemDetailDescription != null)
        itemDetailDescription.text = "";

    if (itemDescriptionName != null)
        itemDescriptionName.text = "";
}

}
