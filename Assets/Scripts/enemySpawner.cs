using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private BoxCollider spawnArea;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxEnemies = 10;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            int currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (currentEnemies < maxEnemies)
            {
                Vector3 spawnPos = GetRandomPointInBox(spawnArea);
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetRandomPointInBox(BoxCollider box)
    {
        Vector3 center = box.center + box.transform.position;
        Vector3 size = box.size;

        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float y = center.y; 
        float z = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        return new Vector3(x, y, z);
    }
}
