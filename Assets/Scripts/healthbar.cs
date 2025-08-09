using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;

    public void SetMaxHealth(float maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
        }
    }

    public void SetHealth(float health)
    {
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }
    }
}