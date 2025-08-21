using UnityEngine;
using UnityEngine.UI;

public class GasMaskUi : MonoBehaviour
{
    public static GasMaskUi instance;

    public Image slotImage; // imaginea din UI
    public Sprite gasMaskIcon; // setata din Inspector
    private GameObject equippedGasMask; // referinta la obiectul din scena
    public Transform fpsCam; // referinta la camera fps

    public bool isGasMaskEquipped = false; // adaugat
    //public task_manager taskManager; // task manager pentru misiune
    public int completMision = 0;

    void Awake()
    {
        instance = this;
        slotImage.enabled = false; // ascuns la inceput


    }

    // Echipare masca
    public void EquipGasMask(GameObject maskObject)
    {
        equippedGasMask = maskObject;
        slotImage.sprite = gasMaskIcon;
        slotImage.enabled = true;
        isGasMaskEquipped = true; // marchez ca masca e activa
        Debug.Log("EquipGasMask called with: " + maskObject);

        completMision++;

        // Marcheaza task-ul cu masca
        //if (taskManager != null)
        //{
        //    taskManager.CompleteTaskByKeyword("Gaseste o masca pentru a completa exchipamentul de exterior.");
        //    Debug.LogWarning("task complet masca");
        //}
    }

    // Dezechipare masca (drop)
    public void UnequipGasMask()
    {
        if (equippedGasMask != null && fpsCam != null)
        {
            equippedGasMask.transform.SetParent(null);
            equippedGasMask.transform.position = fpsCam.position + fpsCam.forward * 1f;
            equippedGasMask.SetActive(true);

            Rigidbody rb = equippedGasMask.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddForce(fpsCam.forward * 2f, ForceMode.Impulse);
            }

            equippedGasMask = null;
        }
        else
        {
            Debug.LogWarning("UnequipGasMask: either equippedGasMask or fpsCam is null.");
        }

        isGasMaskEquipped = false; // masca nu mai e activa
        slotImage.enabled = false;
    }

    // apelat la click pe UI
    public void OnSlotClick()
    {
        UnequipGasMask();
    }
}


