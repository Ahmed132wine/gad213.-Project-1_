using UnityEngine;

public class AngryBird : MonoBehaviour
{
    [Header("Movement")]
    public float flySpeedBoost = 10f;
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
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<LaneMovement3D>();
            foodMeter = player.GetComponent<FoodMeter>();

            GameObject hoverObj = GameObject.FindGameObjectWithTag("Food");
            if (hoverObj != null) foodTarget = hoverObj.transform;
        }

        Destroy(gameObject, 20f);
    }

    void Update()
    {
        if (isScared || foodTarget == null || playerMovement == null) return;

        if (!isAttached)
        {
            float currentSpeed = playerMovement.forwardSpeed + flySpeedBoost;
            transform.position = Vector3.MoveTowards(transform.position, foodTarget.position, currentSpeed * Time.deltaTime);
            transform.LookAt(foodTarget);

            if (Vector3.Distance(transform.position, foodTarget.position) < attachDistance)
            {
                Attach();
            }
        }
        else
        {
            // Hover with jitter around the exact attachment point
            Vector3 hoverOffset = new Vector3(
                Mathf.Sin(Time.time * 5f) * jitterAmount,
                Mathf.Cos(Time.time * 4f) * jitterAmount,
                0
            );
            transform.localPosition = Vector3.Lerp(transform.localPosition, hoverOffset, Time.deltaTime * 8f);

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
        transform.SetParent(foodTarget);
        // FIX: Ensure the bird starts exactly at the hover point
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        Debug.Log("Bird attached and stealing food!");
    }

    public void GetScared()
    {
        if (isScared) return;
        isScared = true;

        transform.SetParent(null);
        if (rb != null)
        {
            rb.isKinematic = false;
            Vector3 fleeDir = (Vector3.up + Vector3.back + Random.insideUnitSphere).normalized;
            rb.velocity = fleeDir * 15f;
        }
        Destroy(gameObject, 1.5f);
    }
}
