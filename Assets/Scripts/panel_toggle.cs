using UnityEngine;

public class panel_toggle : MonoBehaviour
{
    public GameObject panel;

    public void Start()
    {
        panel.SetActive(false);
    }

    public void ShowPanel()
    {
        if (panel != null)
            panel.SetActive(true);
    }

    public void HidePanel()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void TogglePanel()
    {
        if (panel != null)
            panel.SetActive(!panel.activeSelf);
    }
}