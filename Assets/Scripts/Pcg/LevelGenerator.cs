using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]

    [SerializeField] CameraController  cameraController;
    // Assign the chunk prefab in the Inspector
    [SerializeField] GameObject[] chunkPrefab;
    [SerializeField] GameObject CheckpointChunkPrefab;

    [SerializeField] Transform ChunkParent;
    [SerializeField] ScoreManager scoreManager;

    [Header("Level Settings")]
    [Tooltip("Number of chunks to spawn at the start of the game")]

    [SerializeField] int StartingChunks = 12;
    [SerializeField] int checkpointChunkInterval = 8;
    [Tooltip("Do Not Change Chunk Length Value Unless Chunk Prefab Size Reflects Changes ")]
    [SerializeField] float chunkLength = 10f;
    [SerializeField] float MoveSpeed = 5f;
    [SerializeField] float minMoveSpeed = 2f;
    [SerializeField] float maxMoveSpeed = 20f;

    [SerializeField] float minGravityZ = -22f;
    [SerializeField] float maxGravityZ = -2f;

    List<GameObject> chunks = new List<GameObject>();
    int chunksSpawned = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnStartingChunks();
    }

    void Update()
    {
        MoveChunks();
    }

    public void ChangeChunkMoveSpeed(float speedAmount)
    { 
        float newMoveSpeed = MoveSpeed + speedAmount;
        newMoveSpeed = Mathf.Clamp(newMoveSpeed, minMoveSpeed, maxMoveSpeed);


        if (newMoveSpeed != MoveSpeed)
        {
            MoveSpeed = newMoveSpeed;

            float newGravityZ = Physics.gravity.z - speedAmount;
            newGravityZ = Mathf.Clamp(newGravityZ, minGravityZ, maxGravityZ);
            Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, newGravityZ);

            cameraController.ChangeCameraFOV(speedAmount);
        }



    }

    void SpawnStartingChunks()
    {
        for (int i = 0; i < StartingChunks; i++)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        float SpawnPositionZ = CalculateSpawnPositionZ();

        Vector3 chunkSpawnPosition = new Vector3(transform.position.x, transform.position.y, SpawnPositionZ);
        GameObject chunkToSpawn = ChooseChunkToSpawn();

        GameObject newChunkGO = Instantiate(chunkToSpawn, chunkSpawnPosition, Quaternion.identity, ChunkParent);

        chunks.Add(newChunkGO);
        Chunks newChunk = newChunkGO.GetComponent<Chunks>();
        newChunk.Init(this, scoreManager);

        chunksSpawned++;
    }

    private GameObject ChooseChunkToSpawn()
    {
        GameObject chunkToSpawn;

        if (chunksSpawned % checkpointChunkInterval == 0 && chunksSpawned != 0)
        {
            chunkToSpawn = CheckpointChunkPrefab;

        }
        else
        {
            chunkToSpawn = chunkPrefab[Random.Range(0, chunkPrefab.Length)];

        }

        return chunkToSpawn;
    }

    float CalculateSpawnPositionZ()
    {
        float SpawnPositionZ;
        if (chunks.Count == 0)
        {
            SpawnPositionZ = transform.position.z;
        }
        else
        {
            //SpawnPositionZ = transform.position.z + (i * chunkLength);
            SpawnPositionZ = chunks[chunks.Count - 1].transform.position.z + chunkLength;
        }
        return SpawnPositionZ;
    }

    void MoveChunks()
    {
        if (chunks.Count == 0 || Camera.main == null)
            return;

        // iterate backwards so we can safely remove while iterating
        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            GameObject chunk = chunks[i];
            if (chunk == null)
            {
                chunks.RemoveAt(i);
                continue;
            }

            chunk.transform.Translate(-transform.forward * MoveSpeed * Time.deltaTime);

            if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)
            {
                // destroy and remove from list
                Destroy(chunk);
                chunks.RemoveAt(i);
                SpawnChunk(); //add a new chunk when one is destroyed
            }
        }
    }
}