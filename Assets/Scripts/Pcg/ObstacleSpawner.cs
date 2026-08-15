using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float obstacleSpawnTime = 1f;
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Transform obstacleParent;
    [SerializeField] private float spawnWidth = 4f;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float destroyDistance = -20f;

    private List<GameObject> spawnedObstacles = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnObjectRoutine());
    }

    private void Update()
    {
        MoveObstacles();
    }

    IEnumerator SpawnObjectRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(obstacleSpawnTime);

            GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnWidth, spawnWidth),
                transform.position.y,
                transform.position.z
            );

            GameObject obstacle = Instantiate(
                obstaclePrefab,
                spawnPosition,
                Quaternion.identity,
                obstacleParent
            );

            spawnedObstacles.Add(obstacle);
        }
    }

    private void MoveObstacles()
    {
        for (int i = spawnedObstacles.Count - 1; i >= 0; i--)
        {
            if (spawnedObstacles[i] == null)
            {
                spawnedObstacles.RemoveAt(i);
                continue;
            }

            // Agar ulta move kare to Vector3.back ko Vector3.forward kar dena
            spawnedObstacles[i].transform.Translate(
                Vector3.back * moveSpeed * Time.deltaTime,
                Space.World
            );

            if (spawnedObstacles[i].transform.position.z <= destroyDistance)
            {
                Destroy(spawnedObstacles[i]);
                spawnedObstacles.RemoveAt(i);
            }
        }
    }
}