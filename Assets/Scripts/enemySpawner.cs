using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public float spawnRadius = 5f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Vector3 spawnPos = GetNavMeshPosition(transform.position, spawnRadius);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetNavMeshPosition(Vector3 origin, float radius)
    {
        if (NavMesh.SamplePosition(origin, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            Debug.LogWarning("Nu am gasit NavMesh în jurul spawnerului!");
            return origin;
        }
    }
}