using UnityEngine;

public class laborator_computer_menu : MonoBehaviour
{
    public GameObject singleImage;  // imaginea ta, o singura

    private bool isVisible = false;

    // Daca vrei să afisezi imaginea
     public void ShowImage()
    {
        if (singleImage != null && !singleImage.activeSelf)
            singleImage.SetActive(true);
    }

    // Dacă vrei să ascunzi imaginea
    public void HideImage()
    {
        isVisible = false;
        if (singleImage != null)
            singleImage.SetActive(false);
    }
}
