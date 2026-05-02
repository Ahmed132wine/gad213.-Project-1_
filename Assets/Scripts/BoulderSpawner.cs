using UnityEngine;

public class BoulderSpawner : MonoBehaviour
{
    public GameObject boulderPrefab;
    public GameObject warningPrefab; 
    public float spawnDistanceAhead = 50f;
    public float spawnHeight = 20f;
    public float spawnInterval = 8f;
    public float laneWidth = 2.5f;

    private Transform player;
    private float nextSpawnTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nextSpawnTime = Time.time + 3f;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnSequence();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnSequence()
    {
        // 1. Pick a lane
        int lane = Random.Range(0, 3);
        float xPos = (lane - 1) * laneWidth;
        float zPos = player.position.z + spawnDistanceAhead;

        // 2. Spawn Warning Indicator on the ground
        if (warningPrefab != null)
        {
            Vector3 warnPos = new Vector3(xPos, 0.1f, zPos);
            GameObject warning = Instantiate(warningPrefab, warnPos, Quaternion.identity);
            Destroy(warning, 2.5f); // Destroy right before/as boulder hits
        }

        // 3. Spawn Boulder high in the air
        Vector3 spawnPos = new Vector3(xPos, spawnHeight, zPos);
        Instantiate(boulderPrefab, spawnPos, Random.rotation);
    }
}
