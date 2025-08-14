using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public Slider hungerSlider;

    public void SetMaxHunger(float maxHunger)
    {
        if (hungerSlider != null)
        {
            hungerSlider.maxValue = maxHunger;
        }
    }

    public void SetHunger(float hunger)
    {
        if (hungerSlider != null)
        {
            hungerSlider.value = hunger;
        }
    }
}
