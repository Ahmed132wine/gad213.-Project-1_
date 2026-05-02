using UnityEngine;

public class FallingBoulder : MonoBehaviour
{
    [Header("Fall Settings")]
    public float fallGravityScale = 3f; // Makes it feel heavy
    public GameObject impactParticle;

    [Header("Damage Settings")]
    public int foodLoss = 15;
    public float speedPenalty = 0.3f;

    private Rigidbody rb;
    private bool hasHitGround = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Increase gravity so it doesn't float down like a balloon
        rb.drag = 0;
        rb.mass = 100;

        // Safety: Destroy if missed
        Destroy(gameObject, 10f);
    }

    void FixedUpdate()
    {
        // Apply extra downward force for a "heavy" fall
        if (!hasHitGround)
        {
            rb.AddForce(Vector3.down * fallGravityScale, ForceMode.Acceleration);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasHitGround) return;

        if (collision.collider.CompareTag("Player"))
        {
            // CRASH LOGIC
            collision.gameObject.GetComponent<LaneMovement3D>()?.ResetSpeed(speedPenalty);
            collision.gameObject.GetComponent<FoodMeter>()?.LoseFood(foodLoss);

            // Trigger Camera Shake
            Camera.main.GetComponent<CameraFollow>()?.TriggerShake(0.4f, 0.7f);

            Impact();
        }
        else if (collision.collider.CompareTag("Ground"))
        {
            // Landed on road
            Impact();
        }
    }

    void Impact()
    {
        hasHitGround = true;
        if (impactParticle != null)
        {
            Instantiate(impactParticle, transform.position, Quaternion.identity);
        }

        // After hitting the ground, the boulder becomes a stationary obstacle 
        // that disappears shortly after the player passes it.
        Destroy(gameObject, 3f);
    }
}
