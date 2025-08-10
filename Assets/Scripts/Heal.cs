using UnityEngine;

public class Heal : MonoBehaviour
{
    public float healAmount = 5f;
    public float healInterval = 2f;
    private float healTimer;

    public float rayLength = 2f;
    public float rayOffset = 0.1f;
    public LayerMask safeZoneLayer;

    private bool isInSafeZone = false;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        healTimer = healInterval;
    }

    void Update()
    {
        Vector3 origin = transform.position + Vector3.up * rayOffset;
        RaycastHit hit;

       
        isInSafeZone = Physics.Raycast(origin, Vector3.down, out hit, rayLength, safeZoneLayer);

        if (isInSafeZone)
        {
            healTimer -= Time.deltaTime;
            if (healTimer <= 0f)
            {
                playerHealth.Heal(healAmount);
                healTimer = healInterval;
            }
        }
        else
        {
            healTimer = healInterval; 
        }
    }
}