using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    private Door Door;

    private void OnTriggerEnter(Collider other)
    {

        // daca detecteaza camera/caracterul in boxtriggerul pus usi atunci o deschide
        // other.GetComponent<PlayerCam>() != null se poate schimba in componente cu orice componenta a caracterului sau se oate face un tag de player
        // daca nu exista pe caracter si sa se puna other.CompareTag("Player")
        if (other.GetComponent<PlayerCam>() != null)
        {
            if (!Door.IsOpen) {
                Door.Open(other.transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // daca detecteaza camera/caracterul in afara boxtriggerul pus usi atunci o inchide
        if (other.GetComponent<PlayerCam> () != null)
        {
            if (Door.IsOpen)
            {
                Door.Close();
            }
        }
    }
}
