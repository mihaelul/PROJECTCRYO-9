using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionUIManager : MonoBehaviour
{
    // Manager pentru a controla coada de misiuni cand se afiseaza imaginile, cand faci mai multe task uri odata sa apara una dupa
    // alta ca sa nu se suprapuna imaginile
    public static MissionUIManager instance;

    public Image missionImage; // imaginea UI
    private Queue<Sprite> missionQueue = new Queue<Sprite>();
    private bool isShowing = false;

    void Awake()
    {
        instance = this;
        missionImage.enabled = false;
    }

    // afiseaza misiune
    public void ShowMission(Sprite sprite, float duration = 3f)
    {
        missionQueue.Enqueue(sprite);

        if (!isShowing)
        {
            StartCoroutine(ProcessQueue(duration));
        }
    }

    private IEnumerator ProcessQueue(float duration)
    {
        isShowing = true;

        while (missionQueue.Count > 0)
        {
            missionImage.sprite = missionQueue.Dequeue();
            missionImage.enabled = true;

            yield return new WaitForSeconds(duration);

            missionImage.enabled = false;
        }

        isShowing = false;
    }
}
