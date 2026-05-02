using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab;
    public Transform spawnPoint; // Drag the 'BirdSpawnPoint' child here
    public int maxBirds = 3;
    public float spawnInterval = 5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            int currentBirds = GameObject.FindGameObjectsWithTag("Bird").Length;

            if (currentBirds < maxBirds)
            {
                SpawnBird();
            }
            timer = 0;
        }
    }

    void SpawnBird()
    {
        if (birdPrefab == null || spawnPoint == null) return;

        // The spawnPoint is already moving with the player, 
        // so we just instantiate right there with a small random spread.
        Vector3 spread = new Vector3(Random.Range(-2f, 2f), Random.Range(-1f, 1f), 0);
        Instantiate(birdPrefab, spawnPoint.position + spread, Quaternion.identity);
    }
}