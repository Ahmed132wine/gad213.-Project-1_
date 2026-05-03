using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject npcPrefab;
    public float spawnInterval = 1.2f;  // FASTER spawn = MORE cars
    public float spawnDistance = 25f;

    [Header("Max Cars")]
    public int maxCarsOnRoad = 12;  // INCREASED for more traffic

    private Transform player;
    private float nextSpawnTime;
    private int currentCars = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nextSpawnTime = Time.time + 0.5f;  // Quick first spawn
    }

    void Update()
    {
        if (player == null) return;

        // Count current NPC cars
        GameObject[] allNPCs = GameObject.FindGameObjectsWithTag("NPC");
        currentCars = allNPCs.Length;

        if (Time.time >= nextSpawnTime && currentCars < maxCarsOnRoad)
        {
            SpawnNPC();
            // Short random interval for more cars
            nextSpawnTime = Time.time + Random.Range(0.8f, spawnInterval + 0.5f);
        }
    }

    void SpawnNPC()
    {
        if (npcPrefab == null || player == null) return;

        // Spawn ahead of player
        Vector3 spawnPos = player.position + Vector3.forward * spawnDistance;
        spawnPos.y = 0;

        
        float randomX = Random.Range(-2.5f, 2.5f);
        spawnPos.x = randomX;

        Instantiate(npcPrefab, spawnPos, Quaternion.identity);
        Debug.Log("NPC Spawned. Total cars: " + (currentCars + 1));
    }
}
