using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [Header("Chunk Prefabs")]
    public GameObject[] levelChunks; // The random segments for this specific level
    public GameObject startChunk;    // The very first piece of road
    public GameObject finishChunk;   // The piece containing the GoalLine

    [Header("Level Settings")]
    public int maxChunks = 20;       // Total chunks before the finish line
    public float chunkLength = 100f; // Exact length of your road piece
    public int visibleChunks = 5;    // How many chunks to keep at once

    private Transform player;
    private float spawnZ = 0f;
    private int chunksSpawned = 0;
    private bool finishSpawned = false;

    // Queue to track active chunks so we can delete them as we go
    private Queue<GameObject> activeChunks = new Queue<GameObject>();

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        // 1. Spawn Start Chunk
        SpawnChunk(startChunk);

        // 2. Initial Fill
        for (int i = 0; i < visibleChunks - 1; i++)
        {
            SpawnNextRandomChunk();
        }
    }

    void Update()
    {
        if (player == null) return;

        // Check if we need to spawn a new piece as the player moves forward
        if (player.position.z > (spawnZ - (visibleChunks * chunkLength)))
        {
            if (chunksSpawned < maxChunks)
            {
                SpawnNextRandomChunk();
            }
            else if (!finishSpawned)
            {
                SpawnChunk(finishChunk);
                finishSpawned = true;
            }

            // Cleanup chunks behind us to keep memory low
            DeleteOldestChunk();
        }
    }

    void SpawnNextRandomChunk()
    {
        if (levelChunks.Length == 0) return;
        int index = Random.Range(0, levelChunks.Length);
        SpawnChunk(levelChunks[index]);
        chunksSpawned++;
    }

    void SpawnChunk(GameObject prefab)
    {
        if (prefab == null) return;

        GameObject spawned = Instantiate(prefab, Vector3.forward * spawnZ, Quaternion.identity);
        activeChunks.Enqueue(spawned);
        spawnZ += chunkLength;
    }

    void DeleteOldestChunk()
    {
        // Only delete if we have more than the visible limit
        if (activeChunks.Count > visibleChunks + 1)
        {
            GameObject old = activeChunks.Dequeue();
            Destroy(old);
        }
    }
}
