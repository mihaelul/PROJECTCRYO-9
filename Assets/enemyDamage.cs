using UnityEngine;

public class enemyDamage : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Coliziune detectata");

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Lovit playerul!");

            PlayerCam player = collision.gameObject.GetComponent<PlayerCam>();
            
            if (player != null)
            {
                player.TakeDamage(10f);
            }
        }
    }
}