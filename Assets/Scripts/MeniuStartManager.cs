using UnityEngine;
using UnityEngine.UI;

public class MeniuStartManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelStart;
    public GameObject panelMeniu;

    [Header("Content")]
    public GameObject textIntroducere;
    public GameObject imageTaste;

    public void OnStartButton()
    {
        panelStart.SetActive(false);
        panelMeniu.SetActive(true);
    }

    public void OnIntroducereButton()
    {
        textIntroducere.SetActive(true);
        imageTaste.SetActive(false);
    }

    public void OnTasteButton()
    {
        textIntroducere.SetActive(false);
        imageTaste.SetActive(true);
    }

    public void OnNextButton()
    {
        panelMeniu.SetActive(false);
    }
}
