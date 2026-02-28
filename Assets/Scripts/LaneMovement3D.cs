using UnityEngine;

public class LaneMovement3D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f;
    public float laneSwitchSpeed = 8f;
    public float jumpForce = 8f;

    [Header("Lane Settings")]
    public float laneWidth = 2f;
    public int currentLane = 1;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private float targetLaneX;
    private bool isGrounded;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Create ground check object if not assigned
        if (groundCheck == null)
        {
            GameObject check = new GameObject("GroundCheck");
            check.transform.parent = transform;
            check.transform.localPosition = new Vector3(0, -0.8f, 0);
            groundCheck = check.transform;
        }

        // Set initial lane position
        UpdateTargetLaneX();
    }

    void Update()
    {
        // Handle lane switching input (A and D)
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentLane = Mathf.Max(0, currentLane - 1);
            UpdateTargetLaneX();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentLane = Mathf.Min(2, currentLane + 1);
            UpdateTargetLaneX();
        }

        // Handle jump input (Space)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Check if grounded
        CheckGrounded();

        // Move forward automatically (W key)
        if (Input.GetKey(KeyCode.W))
        {
            rb.MovePosition(rb.position + Vector3.forward * forwardSpeed * Time.fixedDeltaTime);
        }

        // Smoothly move to target lane position
        Vector3 currentPos = rb.position;
        Vector3 targetPos = new Vector3(targetLaneX, currentPos.y, currentPos.z);
        Vector3 newPos = Vector3.Lerp(currentPos, targetPos, laneSwitchSpeed * Time.fixedDeltaTime);

        rb.MovePosition(new Vector3(newPos.x, currentPos.y, newPos.z));
    }

    // Define the method BEFORE it's called (moved up)
    void UpdateTargetLaneX()
    {
        // Convert lane number to X position
        if (currentLane == 0)
            targetLaneX = -laneWidth;
        else if (currentLane == 1)
            targetLaneX = 0;
        else if (currentLane == 2)
            targetLaneX = laneWidth;
    }

    void CheckGrounded()
    {
        // Check if sphere is touching ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // Visualize ground check in editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}