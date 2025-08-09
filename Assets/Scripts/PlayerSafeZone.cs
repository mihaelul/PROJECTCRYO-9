using UnityEngine;

public class PlayerSafeZone : MonoBehaviour
{
    public float damageOutsideZone = 5f;
    public float damageInterval = 2f;
    private float damageTimer;

    private bool isInSafeZone = true;
    private PlayerHealth playerHealth; 

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        damageTimer = damageInterval;

   
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);
        bool foundSafeZone = false;

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("SafeZone"))
            {
                foundSafeZone = true;
                break;
            }
    }

    isInSafeZone = foundSafeZone;
    }

    void Update()
    {
        if (!isInSafeZone)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0f)
            {
                playerHealth.TakeDamage(damageOutsideZone);
                damageTimer = damageInterval;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            isInSafeZone = true;
            Debug.Log("player a intrat in zona sigura.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            isInSafeZone = false;
            Debug.Log("player a iesit din zona sigura.");
        }
    }
}