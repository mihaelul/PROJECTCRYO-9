using UnityEngine;

public class PlayerSafeZone : MonoBehaviour
{
    private PlayerCam playerHealth; 
    public float damageOutsideZone = 5f;
    public float damageInterval = 2f;
    private float damageTimer;

    void Start()
    {
        playerHealth = GetComponent<PlayerCam>();
        damageTimer = damageInterval;
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

    private bool isInSafeZone = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            isInSafeZone = true;
            Debug.Log("Player intrat in zona sigura.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            isInSafeZone = false;
            Debug.Log("Player a iesit din zona sigura.");
        }
    }
}
