using UnityEngine;

public class AngryBird : MonoBehaviour
{
    [Header("Movement")]
    public float catchUpSpeed = 15f; 
    public float attachDistance = 1.0f;
    public float jitterAmount = 0.15f;

    [Header("Damage")]
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

            
            Transform[] allChildren = player.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.CompareTag("Food"))
                {
                    foodTarget = child;
                    break;
                }
            }

            
            if (foodTarget == null) foodTarget = player.transform;
        }

        
        Destroy(gameObject, 15f);
    }

    void Update()
    {
        if (isScared || foodTarget == null) return;

        if (!isAttached)
        {

            float currentMoveSpeed = (playerMovement != null ? playerMovement.forwardSpeed : 0) + catchUpSpeed;

           
            transform.position = Vector3.MoveTowards(transform.position, foodTarget.position, currentMoveSpeed * Time.deltaTime);

            
            transform.LookAt(foodTarget.position);

            
            if (Vector3.Distance(transform.position, foodTarget.position) < attachDistance)
            {
                Attach();
            }
        }
        else
        {
            
            Vector3 hoverOffset = new Vector3(
                Mathf.Sin(Time.time * 7f) * jitterAmount,
                Mathf.Cos(Time.time * 6f) * jitterAmount,
                Mathf.Sin(Time.time * 5f) * (jitterAmount / 2f)
            );

            
            transform.localPosition = hoverOffset;
            transform.localRotation = Quaternion.identity;

            
            if (Time.time >= nextDamageTime)
            {
                if (foodMeter != null) foodMeter.LoseFood(foodDamage);
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }

    void Attach()
    {
        if (isAttached) return;
        isAttached = true;

        
        transform.SetParent(foodTarget);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        Debug.Log("Bird has landed on the delivery box!");
    }

    public void GetScared()
    {
        if (isScared) return;
        isScared = true;

        transform.SetParent(null);
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            
            rb.AddForce(Vector3.up * 5f + Vector3.back * 5f, ForceMode.Impulse);
        }
        Destroy(gameObject, 2f);
    }
}
