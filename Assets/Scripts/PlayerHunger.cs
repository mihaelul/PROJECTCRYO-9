using UnityEngine;

public class PlayerHunger : MonoBehaviour
{
    public float maxHunger = 100f;
    public float currentHunger;

    public HungerBar hungerBar;

    private void Start()
    {
        currentHunger = maxHunger;
        hungerBar.SetMaxHunger(maxHunger);
        StartCoroutine(DecreaseHungerOverTime());
    }

    private System.Collections.IEnumerator DecreaseHungerOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); 
            ChangeHunger(-0.3f);
        }
    }

    public void ChangeHunger(float amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger); 
        hungerBar.SetHunger(currentHunger);
    }
}