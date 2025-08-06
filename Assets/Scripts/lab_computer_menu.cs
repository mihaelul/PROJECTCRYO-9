using UnityEngine;

public class FrontBackManager : MonoBehaviour
{
    [System.Serializable]
    public class MovableObject
    {
        public string name;
        public GameObject obj;
    }

    public MovableObject[] objects;
    public float frontZ = -0.1f;
    public float backZ = 1f;

    private int currentFrontIndex = -1;

    public void MoveObjectToFront(int index)
    {
        if (index < 0 || index >= objects.Length)
        {
            Debug.LogWarning("Index invalid.");
            return;
        }

        // Dacă deja este în față, nu face nimic
        if (currentFrontIndex == index) return;

        for (int i = 0; i < objects.Length; i++)
        {
            var obj = objects[i].obj;
            if (obj == null) continue;

            Vector3 pos = obj.transform.position;
            pos.z = (i == index) ? frontZ : backZ;
            obj.transform.position = pos;
        }

        currentFrontIndex = index;
    }
}
