using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject[] chunkPrefab;
    [SerializeField] private GameObject CheckpointChunkPrefab;
    [SerializeField] private Transform ChunkParent;
    [SerializeField] private ScoreManager scoreManager;

    [Header("Level Settings")]
    [SerializeField] private int StartingChunks = 12;
    [SerializeField] private int checkpointChunkInterval = 8;

    [Tooltip("Do Not Change Chunk Length Unless Chunk Prefab Size Reflects Changes")]
    [SerializeField] private float chunkLength = 10f;

    [Header("Chunk Speed")]
    [SerializeField] private float MoveSpeed = 5f;
    [SerializeField] private float minMoveSpeed = 2f;
    [SerializeField] private float maxMoveSpeed = 20f;

    private List<GameObject> chunks = new List<GameObject>();
    private int chunksSpawned = 0;

    private void Start()
    {
        SpawnStartingChunks();
    }

    private void Update()
    {
        MoveChunks();
    }

    // Called when Apple is collected or player hits obstacle
    public void ChangeChunkMoveSpeed(float speedAmount)
    {
        float newMoveSpeed = MoveSpeed + speedAmount;

        newMoveSpeed = Mathf.Clamp(
            newMoveSpeed,
            minMoveSpeed,
            maxMoveSpeed
        );

        if (newMoveSpeed != MoveSpeed)
        {
            MoveSpeed = newMoveSpeed;

            // Change camera FOV
            if (cameraController != null)
            {
                cameraController.ChangeCameraFOV(speedAmount);
            }
        }
    }

    private void SpawnStartingChunks()
    {
        for (int i = 0; i < StartingChunks; i++)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        float spawnPositionZ = CalculateSpawnPositionZ();

        Vector3 chunkSpawnPosition = new Vector3(
            transform.position.x,
            transform.position.y,
            spawnPositionZ
        );

        GameObject chunkToSpawn = ChooseChunkToSpawn();

        GameObject newChunkGO = Instantiate(
            chunkToSpawn,
            chunkSpawnPosition,
            Quaternion.identity,
            ChunkParent
        );

        chunks.Add(newChunkGO);

        Chunks newChunk = newChunkGO.GetComponent<Chunks>();

        if (newChunk != null)
        {
            newChunk.Init(this, scoreManager);
        }

        chunksSpawned++;
    }

    private GameObject ChooseChunkToSpawn()
    {
        GameObject chunkToSpawn;

        if (chunksSpawned % checkpointChunkInterval == 0 &&
            chunksSpawned != 0)
        {
            chunkToSpawn = CheckpointChunkPrefab;
        }
        else
        {
            chunkToSpawn =
                chunkPrefab[Random.Range(0, chunkPrefab.Length)];
        }

        return chunkToSpawn;
    }

    private float CalculateSpawnPositionZ()
    {
        if (chunks.Count == 0)
        {
            return transform.position.z;
        }

        return chunks[chunks.Count - 1].transform.position.z + chunkLength;
    }

    private void MoveChunks()
    {
        if (chunks.Count == 0 || Camera.main == null)
            return;

        // Iterate backwards so chunks can safely be removed
        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            GameObject chunk = chunks[i];

            if (chunk == null)
            {
                chunks.RemoveAt(i);
                continue;
            }

            // Move chunks towards the player
            chunk.transform.Translate(
                -transform.forward *
                MoveSpeed *
                Time.deltaTime
            );

            // Destroy old chunk
            if (chunk.transform.position.z <=
                Camera.main.transform.position.z - chunkLength)
            {
                Destroy(chunk);

                chunks.RemoveAt(i);

                // Spawn replacement chunk
                SpawnChunk();
            }
        }
    }
}