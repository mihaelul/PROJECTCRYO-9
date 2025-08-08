using UnityEngine;

public class ShooterGun : MonoBehaviour
{
    public Transform firePoint;       // locul de unde pleaca glontul
    public GameObject bulletPrefab;   // prefab-ul glontului
    public float bulletForce = 1000f; // forta cu care impingem glontul

    public float fireRate = 0.2f;
    private float nextTimeToFire = 0f;

    public ParticleSystem muzzleFlash;

    void Update()
    {
        // Poti trage doar daca pistolul e in mana
        if (DropAndPickUpItem.currentHeldItem == GetComponent<DropAndPickUpItem>())
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        // Cream glontul la pozitia firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(firePoint.forward * bulletForce);
        }
    }

}
