using UnityEngine;

public class FallingBoulder : MonoBehaviour
{
    [Header("Fall Settings")]
    public float fallGravityScale = 3f;
    public GameObject impactParticle;

    [Header("Damage Settings")]
    public int foodLoss = 15;
    public float speedPenalty = 0.3f;
    public AudioSource crashSound;

    [Header("Camera Shake")]
    public float shakeDuration = 0.4f;
    public float shakeMagnitude = 0.7f;

    private Rigidbody rb;
    private bool hasLanded = false;
    private bool hasDamagedPlayer = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 0;
        rb.mass = 100;
        Destroy(gameObject, 10f);
    }

    void FixedUpdate()
    {
        if (!hasLanded)
        {
            rb.AddForce(Vector3.down * fallGravityScale, ForceMode.Acceleration);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground") && !hasLanded)
        {
            LandOnGround();
        }

        // Player collides with boulder AFTER it has landed
        if (collision.collider.CompareTag("Player") && hasLanded && !hasDamagedPlayer)
        {
            ApplyDamageToPlayer(collision.gameObject);
        }
    }

    void LandOnGround()
    {
        hasLanded = true;
        Debug.Log("Boulder landed on ground");

        // Freeze the boulder so it becomes static obstacle
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Keep collider active
        GetComponent<Collider>().isTrigger = false;

        // Particle effect
        if (impactParticle != null)
        {
            Instantiate(impactParticle, transform.position, Quaternion.identity);
        }

        // Optional: ground impact sound
        // if (crashSound != null) crashSound.PlayOneShot(crashSound.clip);

        // Destroy after enough time for player to potentially hit it
        Destroy(gameObject, 5f);
    }

    void ApplyDamageToPlayer(GameObject player)
    {
        hasDamagedPlayer = true;
        Debug.Log("Player hit the landed boulder!");

        // Speed penalty
        LaneMovement3D movement = player.GetComponent<LaneMovement3D>();
        if (movement != null) movement.ResetSpeed(speedPenalty);

        // Food loss
        FoodMeter food = player.GetComponent<FoodMeter>();
        if (food != null) food.LoseFood(foodLoss);

        // Camera shake
        Camera.main?.GetComponent<CameraFollow>()?.TriggerShake(shakeDuration, shakeMagnitude);

        // Crash sound
        if (crashSound != null && crashSound.clip != null)
            crashSound.PlayOneShot(crashSound.clip);

        // Optional: destroy boulder upon impact (so player doesn't hit it repeatedly)
        Destroy(gameObject, 0.1f);
    }
}
