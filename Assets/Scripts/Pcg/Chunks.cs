using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Chunks : MonoBehaviour
{
    [SerializeField] GameObject fencePrefab;
    [SerializeField] GameObject applePrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float appleSpawnChance = 0.3f;
    [SerializeField] float coinSpawnChance = 1f;// Chance to spawn an apple (0 to 1)
    [SerializeField] float coinSeperationLength = 2f; // Minimum separation between coins in the same lane
    [SerializeField] float[] lanes = { -2.5f, 0f, 2.5f };


    LevelGenerator levelGenerator;
    ScoreManager scoreManager;

    List<int> availableLanes = new List<int> { 0, 1, 2 };

    void Start()
    {
        SpawnFence();
        SpawnApple();
        SpawnCoin();
    }
     
    public void Init(LevelGenerator levelGenerator, ScoreManager scoreManager)
    {

        this.levelGenerator = levelGenerator;
        this.scoreManager = scoreManager ;
    }

    void SpawnFence()
    {
      
        int fencesToSpawn = Random.Range(0 , lanes.Length); // Randomly choose how many fences to spawn (1 to 3)

        for (int i = 0; i < fencesToSpawn; i++)
        {
            if (availableLanes.Count == 0) break; // No more lanes available

            int selectedLane = SelectedLane();// Remove the selected lane to avoid duplicates
            Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);

            Instantiate(fencePrefab, spawnPosition, Quaternion.identity, this.transform);
        }
    }

    void SpawnApple()
    {
        if (Random.value > appleSpawnChance || availableLanes.Count <=0) return; // Skip spawning apple based on chance

        int selectedLane = SelectedLane();// Remove the selected lane to avoid duplicates
        Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);

        Apple newApple = Instantiate(applePrefab, spawnPosition, Quaternion.identity, this.transform).GetComponent<Apple>();
        newApple.Init(levelGenerator); // Set the lane for the apple
    }

    void SpawnCoin()
    {
        if (Random.value > coinSpawnChance || availableLanes.Count <= 0) return;

        int maxCoinsToSpawn = 6; // Limit the number of coins to spawn based on available lanes
        int coinToSpawn = Random.Range(1, maxCoinsToSpawn); // Randomly choose how many coins to spawn (1 to available lanes)
        
        float topOfChunkZPos = transform.position.z + (coinSeperationLength * 2f);

        int selectedLane = SelectedLane();// Remove the selected lane to avoid duplicates

        for (int i = 0; i < coinToSpawn; i++)
        {
            float spawnpositionZ = topOfChunkZPos - (i * coinSeperationLength);
            Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, spawnpositionZ);

            Coin newCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity, this.transform).GetComponent<Coin>();
            newCoin.Init(scoreManager); // Set the lane for the coin
        }
    }

    int SelectedLane()
    {
        int randomLaneIndex = Random.Range(0, availableLanes.Count);
        int selectedLane = availableLanes[randomLaneIndex];
        availableLanes.RemoveAt(randomLaneIndex);
        return selectedLane;
    }
}
