using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class GasMaskUi : MonoBehaviour
{
    public static GasMaskUi instance;

    public Image slotImage; // imaginea din UI
    public Sprite gasMaskIcon; // setata din Inspector
    private GameObject equippedGasMask; // referinta la obiectul din scena
    public Transform fpsCam; // referinta la camera fps

    public Sprite missionCompletedImage;

    public bool isGasMaskEquipped = false; // adaugat
    public int completMision = 0;


    void Awake()
    {
        instance = this;
        slotImage.enabled = false; // ascuns la inceput

    }

    private void Update()
    {
        // drop la masca pe tasta 1 fara buton pe ecran
        if (isGasMaskEquipped && Input.GetKeyDown(KeyCode.Alpha1))
        {
            UnequipGasMask();
        }
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

        // daca tocmai s-a completat misiunea, afisam imaginea 3 sec
        if (completMision == 1)
        {
            MissionUIManager.instance.ShowMission(missionCompletedImage, 3f);
        }
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

}


