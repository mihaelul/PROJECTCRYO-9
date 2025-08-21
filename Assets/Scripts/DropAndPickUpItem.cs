/*
 PICK-UP AND DROP ITEM din mana

IMPORTANT 

- Obiectul reactioneaza cu corpul caracterului care trebuie sa aiba rigidBody (mai pe scurt daca el e pe jos (obiectul) si caracterul se loveste 
de el cand merge reactioneaza ca un obiect normal si se misca din pozitia lui, se rostogoleste)
- la rigidBody se pune RIgidBody al obiectului, trebuie sa aiba bifat automatic mass, automati tensor, use gravity, is kinematic, la collision detection  se pune CONTINUOUS
- la BoxCollider se pune tot boxcollider-ul obiectului cu trigger activ
- la player se pune player-ul cu rigidbody
- la itemContainer (se face un item gol ca copil la camera - pozitia lui sa vina ca un obiect in mana) si se pune !!!!! PENTRU FIECARE ITEM TREBUIE SA FIE UN CONTAINER DIFERIT!!!!!
- la fpsCam se pune camera, adica parintele containerului
- obiectul vine ca copil al containerului (pozitia lui in layer)
- TASTA E pentru a lua obiect in mana
- TASTA Q pentru a arunca obiectul din mana
- itemOK este DEZACTIVAT la inceput la toate obiectele, ne spune daca putem sau nu arunca obiectul (ex daca e in mana e activ, daca e jos e dezactivat)
- slotFull ne spune daca avem un obiect in mana sau nu
- pickUpRange - distanta de la care functioneaza tastele
- dropForwardForce si UpwardForce puteri fizice de aruncare a obiectului (cat mai mici ca sa nu zboare obiectul la un km)

 */


using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public class DropAndPickUpItem : MonoBehaviour
{

    public Rigidbody rigidBody;
    public BoxCollider BoxCollider;
    public Transform player, itemContainer, fpsCam;

    public float pickUpRange; // de la ce distanta functioneaza tastele
    public float dropForwardForce, dropUpwardForce;

    public bool itemOk; // daca item-ul poate sa fie luat sau nu
    public static bool slotFull; // are deja item in mana sau nu

    // -------- update pick-up item, adaugar pick-up iteme multiple 31.07.2025 -----------
    public static DropAndPickUpItem currentHeldItem = null;

    //------- iza ---------
    public Item itemData;
    public InventoryManager inventoryManager;

    private void Start()
    {
        // setUp
        // la inceput itemul nu are parinti, parintele null(sa nu se miste cu camera la inceput)

        transform.SetParent(null);

        if (!itemOk)
        {
            rigidBody.isKinematic = false;
            BoxCollider.isTrigger = false;
        }
        else
        {
            // ------ update 31.07 ------
            // initializare parametri item ini mana functie
            HoldItem();

        }

    }

    // -------- update 31.07 --------
    // setare parametri in functie de statusul obiecttului folosind enum
    // pe jos, in mana, in inventar
    public enum ItemState { OnGround, InHand, InInventory }

    public void SetState(ItemState state)
    {
        switch (state)
        {
            case ItemState.OnGround:
                itemOk = false;
                slotFull = false;
                currentHeldItem = null;
                gameObject.SetActive(true);
                transform.SetParent(null);
                rigidBody.isKinematic = false;
                BoxCollider.isTrigger = false;
                break;

            case ItemState.InHand:
                itemOk = true;
                slotFull = true;
                currentHeldItem = this;
                HoldItem();
                break;

            case ItemState.InInventory:
                itemOk = false;
                slotFull = false;
                currentHeldItem = null;
                gameObject.SetActive(false);
                transform.SetParent(null);
                break;
        }
    }


    public void PickUp() {

        // pentru masca de gaz special 
        if (itemData != null && itemData.itemName == "Mask")
        {
            // masca -> logica speciala
            SetState(ItemState.InInventory); // doar ca sa dispara din scena, dar nu o pune in inventar si nici nu o distruge
            GasMaskUi.instance.EquipGasMask(this.gameObject);
            this.gameObject.SetActive(false);
            return;
        }

        // mancare, pentru misiuni trimite semnal ca a luat mancare de jos
        FoodMission.instance.OnFoodCollected(this);


        // ------- update 31.07 -----
        // daca avem un obiect in mana si primim semnal de a lua altul, itemul din mana se duce in inventar
        // Itemul din mana nu se aduga in inventar el se aduga abaia cand se ia alt item in mana altfel s-ar pune de 2 ori in inventar
        if (slotFull && currentHeldItem != null)
        {
            currentHeldItem.PutInInventory();
        }

        // setare parametri cand e in mana urmatorul item sau primul item luat in mana
        SetState(ItemState.InHand);
    }

    // functie setare parametri cand e in mana
    private void HoldItem()
    {
        transform.SetParent(itemContainer, worldPositionStays: false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        rigidBody.isKinematic = true;
        BoxCollider.isTrigger = true;
    }

    public void Drop() {
        // ----- update 31.07 ------
        // setare parametri cand vrem sa aruncam itemul din mana
        SetState(ItemState.OnGround);


        // Mutam usor in fata obiectul, sa nu se suprapuna cu playerul
        transform.position = fpsCam.position + fpsCam.forward * 1f;

        // item-ul contine momentum-ul de la player(forta fizica)
        rigidBody.linearVelocity = player.GetComponent<Rigidbody>().linearVelocity;

        // adaugam forta
        rigidBody.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);

        rigidBody.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);

        float random = UnityEngine.Random.Range(-1f, 1f);
        rigidBody.AddTorque(new Vector3(random, random, random) * 10);

        // eliminam din inventar itemul
        if (inventoryManager != null && itemData != null)
        {
            inventoryManager.RemoveItemFromSlot(itemData); // fortat
        }

        //update iza
        // daca arunc obiectul din mana(il scot din inventar) sterg descrierea
        if (inventoryManager != null)
        {
            foreach (var slot in inventoryManager.itemSlots)
            {
                slot.ClearDetails();
            }
        }
    }

    //----- update 31.07 ------
    // functie pentru a pune obiectul in inventar
    public void PutInInventory()
    {
        // setam parametrii
        SetState(ItemState.InInventory);

        // adaugam item-ul in inventar
        if (inventoryManager != null && itemData != null)
        {
            inventoryManager.AddItem(itemData, this.gameObject);
        }

    }
    
}
