using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("NPC Collision Penalty")]
    [Range(0f, 1f)]
    public float speedPenaltyMultiplier = 0.4f;
    public int foodPenaltyAmount = 10;
    public float penaltyCooldown = 1.5f;

    [Header("Visual/Audio")]
    public ParticleSystem impactParticles;
    public AudioSource crashSound;

    
    [Header("Screen Shake")]
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.5f;

    private LaneMovement3D playerMovement;
    private FoodMeter playerFoodMeter;
    private float nextPenaltyTime = 0f;

    void Start()
    {
        playerMovement = GetComponent<LaneMovement3D>();
        playerFoodMeter = GetComponent<FoodMeter>();
        if (crashSound == null) crashSound = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision hitData)
    {
        if (Time.time < nextPenaltyTime) return;

        if (hitData.collider.CompareTag("NPC"))
        {
            ApplyImpactEffect();
            nextPenaltyTime = Time.time + penaltyCooldown;
        }
    }

    void ApplyImpactEffect()
    {
        Debug.Log("Hit an NPC car!");

        if (playerMovement != null) playerMovement.ResetSpeed(speedPenaltyMultiplier);
        if (playerFoodMeter != null) playerFoodMeter.LoseFood(foodPenaltyAmount);

        if (impactParticles != null) impactParticles.Play();
        if (crashSound != null && crashSound.clip != null) crashSound.PlayOneShot(crashSound.clip);

        
        if (Camera.main != null)
        {
            CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
            if (camFollow != null)
            {
                camFollow.TriggerShake(shakeDuration, shakeMagnitude);
            }
        }
    }
}
