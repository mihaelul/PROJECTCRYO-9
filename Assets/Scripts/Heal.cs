using UnityEngine;
using System.Collections;

public class Heal : MonoBehaviour
{
    public float healAmount = 5f;
    public float healInterval = 2f;
    private float healTimer;

    private bool isInSafeZone = false;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        healTimer = healInterval;
    }

    void Update()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            isInSafeZone = true;
          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            isInSafeZone = false;
          
        }
    }
}
