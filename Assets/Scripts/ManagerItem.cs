/*
 * Pentru a gestiona mai bine itemele din mana am facut un manager care selecteaza in mana doar itemul din fata camerei
 * ACEST SCRIPT SE PUNE DOAR PE CAMERA 
 * Penru a selecta itemele corespunzator ne folosim de un nou layer unde punem toate obiectele, pe fiecare obiect in partea dreapta sus la layer am creat un layer nou MangaerItem
 */

using UnityEngine;

public class ManagerItem : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask interactLayer; // layerul pe care se afla obiectele (cel cu care o sa interactioneze raza)
    public Camera fpsCamera;


    private void Update()
    {
        // Luam obiectul
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickUp();
        }
        // Aruncam obiectul
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryDrop();
        }
    }
  
    void TryPickUp()
    {
        // ----- update 31.07 -----
        // am eliminat conditia de un singur item in mana

        // directia cu care detectam obiectele din fata camerei
        Ray ray = new Ray(fpsCamera.transform.position, fpsCamera.transform.forward);
        // obiectul pe care raza/directia ray il atinge

        RaycastHit hit;

        // daca este atins pe o distanta data
        if (Physics.Raycast(ray, out hit, interactionDistance - 1, interactLayer))
        {
            // cauta componenta lovita
            DropAndPickUpItem item = hit.collider.GetComponentInParent<DropAndPickUpItem>();

            if (item != null)
            {
                item.PickUp();
            }
        }
    }

    void TryDrop()
    {
        // cautaam toate item ele cu scriptul atasat
        DropAndPickUpItem[] allItems = FindObjectsOfType<DropAndPickUpItem>();
        foreach (var item in allItems)
        {
            // selectam doar itemul care e in mana si ii facem drop
            if (item.itemOk)
            {
                item.Drop();
                break;
            }
        }
    }
}
