using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;
    public float initialSpawnDelay = 5.5f;
    public float minSpawnDelay = 1.5f;
    public float spawnDelayDecrease = 0.5f;

    private Dictionary<Transform, GameObject> occupiedSpawnPoints = new Dictionary<Transform, GameObject>();
    private Queue<GameObject> spawnQueue = new Queue<GameObject>();

    private bool stopSpawning = false;
    private void Start()
    {
        StartCoroutine(SpawnItemWithInterval());
    }

    public void StopSpawning()
    {
        stopSpawning = true;
        // You can add further logic here to stop enemy spawning
    }

    IEnumerator SpawnItemWithInterval()
    {
        float currentSpawnDelay = initialSpawnDelay;
        float spawnTime = 0f;

        while (true)
        {
            if (spawnPoints.Length == 0 || enemyPrefabs.Length == 0)
            {
                Debug.LogError("No spawn points or enemy prefabs assigned!");
                yield break;
            }

            spawnTime += Time.deltaTime;

            foreach (Transform spawnPoint in spawnPoints)
            {
                if (!occupiedSpawnPoints.ContainsKey(spawnPoint) || occupiedSpawnPoints[spawnPoint] == null)
                {
                    int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
                    GameObject enemyPrefabToSpawn = enemyPrefabs[randomEnemyIndex];

                    if (enemyPrefabToSpawn != null)
                    {
                        spawnQueue.Enqueue(enemyPrefabToSpawn);
                    }
                }
            }

            while (spawnQueue.Count > 0)
            {
                if (!stopSpawning) // Add this condition to stop spawning
                {
                    GameObject nextEnemy = spawnQueue.Dequeue();
                    SpawnEnemy(nextEnemy);
                    yield return new WaitForSeconds(currentSpawnDelay);
                    currentSpawnDelay = Mathf.Max(currentSpawnDelay - spawnDelayDecrease, minSpawnDelay);
                }
                else
                {
                    yield break; // Break the coroutine if stopSpawning is true
                }
            }


            if (spawnTime >= 60f)
            {
                int firstRandomIndex = Random.Range(0, enemyPrefabs.Length);
                int secondRandomIndex = Random.Range(0, enemyPrefabs.Length);

                GameObject firstEnemyPrefab = enemyPrefabs[firstRandomIndex];
                GameObject secondEnemyPrefab = enemyPrefabs[secondRandomIndex];

                spawnQueue.Enqueue(firstEnemyPrefab);
                spawnQueue.Enqueue(secondEnemyPrefab);

                spawnTime = 0f;
            }

            yield return null;
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        List<Transform> availableSpawnPoints = new List<Transform>();

        foreach (Transform spawnPoint in spawnPoints)
        {
            if (!occupiedSpawnPoints.ContainsKey(spawnPoint) || occupiedSpawnPoints[spawnPoint] == null)
            {
                availableSpawnPoints.Add(spawnPoint);
            }
        }

        if (availableSpawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[randomIndex];

            Vector3 spawnPosition = spawnPoint.position;
            Quaternion spawnRotation = spawnPoint.rotation;

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, spawnRotation);
            occupiedSpawnPoints[spawnPoint] = newEnemy;
            Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        }
    }


    private Transform GetEmptySpawnPoint()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (!occupiedSpawnPoints.ContainsKey(spawnPoint) || occupiedSpawnPoints[spawnPoint] == null)
            {
                return spawnPoint;
            }
        }
        return null;
    }

    public void EnemyDestroyed(Transform spawnPoint)
    {
        if (occupiedSpawnPoints.ContainsKey(spawnPoint))
        {
            occupiedSpawnPoints[spawnPoint] = null;
        }
    }
}
