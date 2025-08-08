using UnityEngine;

public class ShooterTorpila : MonoBehaviour
{
    public float speed = 50f;
    public float damage = 25f;
    public float lifetime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ShooterItemViata target = collision.gameObject.GetComponent<ShooterItemViata>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
