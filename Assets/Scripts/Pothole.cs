using UnityEngine;

public class Pothole : MonoBehaviour
{
    [Header("Penalty Settings")]
    [Range(0f, 1f)]
    public float speedPenaltyMultiplier = 0.5f;
    public int foodPenaltyAmount = 5;

    [Header("Visual/Audio")]
    public ParticleSystem hitParticles;
    public AudioSource hitSound;

    [Header("Screen Shake (Pothole)")]
    public float shakeDuration = 0.2f;      // Shorter than NPC collision (0.3f)
    public float shakeMagnitude = 0.25f;    // Lower than NPC collision (0.5f)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyPenalty(other.gameObject);
        }
    }

    void ApplyPenalty(GameObject player)
    {
        Debug.Log("Hit a pothole!");

        // 1. Reduce Speed
        LaneMovement3D movement = player.GetComponent<LaneMovement3D>();
        if (movement != null)
        {
            movement.ResetSpeed(speedPenaltyMultiplier);
        }

        // 2. Reduce Food
        FoodMeter foodMeter = player.GetComponent<FoodMeter>();
        if (foodMeter != null)
        {
            foodMeter.LoseFood(foodPenaltyAmount);
        }

        // 3. Play Effects
        if (hitParticles != null) hitParticles.Play();
        if (hitSound != null && hitSound.clip != null) hitSound.Play();

        // 4. Camera Shake (Lower intensity)
        if (Camera.main != null)
        {
            CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
            if (camFollow != null)
            {
                camFollow.TriggerShake(shakeDuration, shakeMagnitude);
                Debug.Log("Pothole camera shake!");
            }
        }

        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 2f);
    }
}
