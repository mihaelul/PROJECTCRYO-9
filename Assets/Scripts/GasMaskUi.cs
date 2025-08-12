using UnityEngine;
using UnityEngine.UI;

public class GasMaskUi : MonoBehaviour
{
    public static GasMaskUi instance;

    public Image slotImage; // imaginea din UI
    public Sprite gasMaskIcon; // setata din Inspector
    private GameObject equippedGasMask; // referinta la obiectul din scena
    public Transform fpsCam; // referinta la camera fps

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
        Debug.Log("EquipGasMask called with: " + maskObject);

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

        slotImage.enabled = false;
    }

    // apelat la click pe UI
    public void OnSlotClick()
    {
        UnequipGasMask();
    }
}


