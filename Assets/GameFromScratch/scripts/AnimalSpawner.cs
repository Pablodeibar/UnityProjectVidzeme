using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject[] animalPrefabs; // Array of animal prefabs to spawn
    public Transform[] spawnPoints; // Array of spawn points
    public float spawnRate = 5f; // Time in seconds between spawns
    public float placementRadius = 1f; // Radius within which to place animals on the NavMesh

    private void Start()
    {
        // Start the coroutine to spawn animals automatically
        StartCoroutine(SpawnAnimals());
    }

    private IEnumerator SpawnAnimals()
    {
        while (true)
        {
            // Wait for the next spawn
            yield return new WaitForSeconds(spawnRate);

            // Select a random spawn point and animal prefab
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject animalPrefab = animalPrefabs[Random.Range(0, animalPrefabs.Length)];

            // Find a valid position on the NavMesh close to the spawn point
            Vector3 spawnPosition = FindValidNavMeshPosition(spawnPoint.position);

            // Instantiate the animal at the selected spawn point
            GameObject newAnimal = Instantiate(animalPrefab, spawnPosition, spawnPoint.rotation);
        }
    }

    private Vector3 FindValidNavMeshPosition(Vector3 originalPosition)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(originalPosition, out hit, placementRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            Debug.LogWarning("Failed to find a valid NavMesh position near the spawn point.");
            return originalPosition; // Fallback to original position if no valid position found
        }
    }
}
