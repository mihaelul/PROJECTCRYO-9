using UnityEngine;

public class ItemPlacement_O2 : MonoBehaviour
{
    [Header("Placement Settings")]
    public Transform placementPosition; // position object will be placed in (tank socket of O2 Distributor)
    private Vector3 placementRotation; // --//-- but with rotation
    private float placementSpeed = 5f;
    private float placementDelay = 0.5f; // delay before starting movement

    [Header("Detection Settings")]
    public float detectionRadius = 0.1f;
    public LayerMask ManageritemLayer;
    private DropAndPickUpItem currentItem;
    private bool isPlacing = false;
    private float placementTime = 0f;

    [Header("External Variables")]

    public task_manager task_manager;

    void Awake()
    {
        if (task_manager == null)
            task_manager = FindObjectOfType<task_manager>();
        task_manager.neededTanks = 2;
        // initialize value of neededTanks - Alex - nu am idee de ce trebuie sa fie init aici, 
        // but it works sooo :/
    }

    void IsItemThrown()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, ManageritemLayer);
        foreach (var hit in hitColliders)
        {
            if (hit.gameObject.name.Contains("OxygenTank")) // check if the thrown object is an oxygen tank
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null && rb.linearVelocity.magnitude > 0.1f) // check if item is being thrown
                {
                    DropAndPickUpItem o2Tank = hit.GetComponent<DropAndPickUpItem>();
                    if (o2Tank != null)
                    {
                        task_manager.neededTanks--;
                        // Debug.Log($"neededTanks current value is {task_manager.neededTanks}");
                        StartPlacement(o2Tank);
                        return;
                    }
                }
            }
        }
    }

    void StartPlacement(DropAndPickUpItem o2Tank)
    {
        currentItem = o2Tank;
        currentItem.itemOk = false; // disable pickup

        // disable physics
        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
        }

        isPlacing = true;
    }

    void PlaceItem()
    {
        currentItem.transform.position = Vector3.Lerp(
                                        currentItem.transform.position,
                                        placementPosition.position,
                                        placementSpeed * Time.deltaTime
                                        );

        Quaternion targetRotation = placementPosition.rotation;

        if (placementRotation != Vector3.zero)
            targetRotation *= Quaternion.Euler(placementRotation);

        currentItem.transform.rotation = Quaternion.Slerp(
                                        currentItem.transform.rotation,
                                        targetRotation,
                                        placementSpeed * Time.deltaTime
                                        );

        float distance = Vector3.Distance(currentItem.transform.position, placementPosition.position);
        float angle = Quaternion.Angle(currentItem.transform.rotation, targetRotation);

        if (distance < 0.01f && angle < 1f)
            CompletePlacement();
    }

    void CompletePlacement()
    {
        currentItem.transform.position = placementPosition.position;
        currentItem.transform.rotation = placementPosition.rotation;

        currentItem.rigidBody.isKinematic = true;
        currentItem.itemOk = false;
        isPlacing = false;
        detectionRadius = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlacing)
            IsItemThrown();
        else
            PlaceItem();
    }
}