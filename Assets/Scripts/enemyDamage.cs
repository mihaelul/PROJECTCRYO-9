using UnityEngine;

public class enemyDamage : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
      

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Lovit playerul!");

            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            
            if (player != null)
            {
                player.TakeDamage(10f);
            }
        }
    }
}