using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
{
    currentHealth += amount;
    currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

    if (healthBar != null)
    {
        healthBar.SetHealth(currentHealth);
    }
}

    void Die()
    {
        Debug.Log("Player a murit.");
    }
}