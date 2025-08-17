using UnityEngine;

public class PlayerSafeZone : MonoBehaviour
{
    public float damageOutsideZone = 5f;
    public float damageInterval = 2f;
    private float damageTimer;

    public float checkRadius = 1f; 
    public float groundOffset = 0.5f; 
    public LayerMask safeZoneLayer;  

    private bool isInSafeZone = true;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        damageTimer = damageInterval;
    }

    void Update()
{
    Vector3 origin = transform.position + Vector3.up * 0.1f; 
    float rayLength = 2f;

    RaycastHit hit;
    if (Physics.Raycast(origin, Vector3.down, out hit, rayLength, safeZoneLayer))
    {
        isInSafeZone = true;
    }
    else
    {
        isInSafeZone = false;
    }

    Debug.Log("In safe zone (Raycast): " + isInSafeZone);
    
    if (!isInSafeZone)
    {
         // verificam daca masca e echipata
         if (GasMaskUi.instance != null && GasMaskUi.instance.isGasMaskEquipped)
         {
                // avem masca -> nu luam damage
                damageTimer = damageInterval; // resetam timerul ca si cum ar fi in safe zone
                Debug.Log("Protejat de masca, fara damage.");
            return;
         }
        
        damageTimer -= Time.deltaTime;
        if (damageTimer <= 0f)
        {
            playerHealth.TakeDamage(damageOutsideZone);
            damageTimer = damageInterval;
        }
    }
    else
    {
        damageTimer = damageInterval;
    }
}


}
