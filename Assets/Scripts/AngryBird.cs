using UnityEngine;

public class AngryBird : MonoBehaviour
{
    [Header("Movement")]
    public float flySpeedBoost = 10f; // How much faster than player
    public float attachDistance = 0.8f;
    public float jitterAmount = 0.2f;

    [Header("Conflict Settings")]
    public int foodDamage = 5;
    public float damageInterval = 2f;

    private Transform foodTarget;
    private LaneMovement3D playerMovement;
    private FoodMeter foodMeter;
    private Rigidbody rb;

    private bool isAttached = false;
    private bool isScared = false;
    private float nextDamageTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Disable physics collisions so it doesn't bump the player
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Find Player and Target
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<LaneMovement3D>();
            foodMeter = player.GetComponent<FoodMeter>();

            // Find the "BirdHoverPoint" we tagged as "Food"
            GameObject hoverObj = GameObject.FindGameObjectWithTag("Food");
            if (hoverObj != null) foodTarget = hoverObj.transform;
        }

        // Cleanup if it gets lost
        Destroy(gameObject, 20f);
    }

    void Update()
    {
        if (isScared || foodTarget == null || playerMovement == null) return;

        if (!isAttached)
        {
            // CHASE LOGIC
            // Calculate speed: Player's speed + our boost
            // Note: If your LaneMovement variable is 'currentForwardSpeed', use that.
            // Based on your previous file, it's 'forwardSpeed'.
            float currentSpeed = playerMovement.forwardSpeed + flySpeedBoost;

            // Move toward the target in world space
            transform.position = Vector3.MoveTowards(transform.position, foodTarget.position, currentSpeed * Time.deltaTime);
            transform.LookAt(foodTarget);

            // Check for attachment
            if (Vector3.Distance(transform.position, foodTarget.position) < attachDistance)
            {
                Attach();
            }
        }
        else
        {
            // ATTACHED/HOVER LOGIC
            // Use localPosition so we move WITH the scooter
            Vector3 hoverOffset = new Vector3(
                Mathf.Sin(Time.time * 5f) * jitterAmount,
                Mathf.Cos(Time.time * 4f) * jitterAmount,
                0
            );
            transform.localPosition = Vector3.Lerp(transform.localPosition, hoverOffset, Time.deltaTime * 2f);

            // Damage the food meter
            if (Time.time >= nextDamageTime)
            {
                if (foodMeter != null) foodMeter.LoseFood(foodDamage);
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }

    void Attach()
    {
        isAttached = true;
        transform.SetParent(foodTarget); // Become a child of the hover point
        transform.localRotation = Quaternion.identity; // Align rotation
        Debug.Log("Bird attached and stealing food!");
    }

    public void GetScared()
    {
        if (isScared) return;
        isScared = true;

        transform.SetParent(null); // Stop moving with player

        if (rb != null)
        {
            rb.isKinematic = false; // Turn physics back on to fly away
            Vector3 fleeDir = (Vector3.up + Vector3.back + Random.insideUnitSphere).normalized;
            rb.velocity = fleeDir * 15f;
        }

        Destroy(gameObject, 1.5f);
    }
}
