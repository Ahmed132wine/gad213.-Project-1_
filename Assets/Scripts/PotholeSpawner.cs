using UnityEngine;

public class PotholeSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject potholePrefab;
    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 5f;
    public float spawnDistanceAhead = 40f; // Spawn far enough ahead so the player has time to react

    [Header("Lane Settings")]
    public float laneWidth = 2.5f; 

    private Transform player;
    private float nextSpawnTime;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        SetNextSpawnTime();
    }

    void Update()
    {
        if (player == null || potholePrefab == null) return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnPothole();
            SetNextSpawnTime();
        }
    }

    void SpawnPothole()
    {
        
        int randomLane = Random.Range(0, 3);

        // Calculate the X position based on the lane
        float spawnX = 0f;
        if (randomLane == 0) spawnX = -laneWidth;
        else if (randomLane == 2) spawnX = laneWidth;

        // Calculate final spawn position
        Vector3 spawnPos = new Vector3(spawnX, 0.05f, player.position.z + spawnDistanceAhead);

        

        GameObject newPothole = Instantiate(potholePrefab, spawnPos, Quaternion.identity);

        // Clean up potholes that are missed to save memory
        Destroy(newPothole, 15f);
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}
