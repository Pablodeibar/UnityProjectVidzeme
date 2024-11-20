using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject[] animalPrefabs; // Array of animal prefabs to spawn
    public Transform[] spawnPoints; // Array of spawn points
    public float spawnRate = 5f; // Time in seconds between spawns

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

            // Instantiate the animal at the selected spawn point
            Instantiate(animalPrefab, spawnPoint.position, spawnPoint.rotation);

            // Optionally, you can play a spawn sound or other effects here
        }
    }
}
