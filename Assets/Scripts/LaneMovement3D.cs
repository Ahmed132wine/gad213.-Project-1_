using UnityEngine;

public class LaneMovement3D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 15f;
    public float laneSwitchSpeed = 12f;
    public float jumpForce = 8f;

    [Header("Lane Settings")]
    public float laneWidth = 2.5f;
    public int currentLane = 1;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private float targetLaneX;
    private bool isGrounded;

    // NEW: Variable to track current speed for collision penalties
    private float currentForwardSpeed;
    private float recoveryRate = 5f; // How fast you regain speed

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentForwardSpeed = forwardSpeed; // Initialize speed

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.freezeRotation = true;

        UpdateTargetLaneX();
    }

    void Update()
    {
        // Lane switching
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentLane = Mathf.Max(0, currentLane - 1);
            UpdateTargetLaneX();
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentLane = Mathf.Min(2, currentLane + 1);
            UpdateTargetLaneX();
        }

        // Jump
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        CheckGrounded();

        // Gradually recover speed if it was lowered by a crash
        if (currentForwardSpeed < forwardSpeed)
            currentForwardSpeed += 5f * Time.fixedDeltaTime;

        Vector3 moveStep = new Vector3(targetLaneX, rb.position.y, rb.position.z + currentForwardSpeed * Time.fixedDeltaTime);
        moveStep.x = Mathf.Lerp(rb.position.x, targetLaneX, laneSwitchSpeed * Time.fixedDeltaTime);

        rb.MovePosition(moveStep);

        
    }

    // THE MISSING METHOD: Called by PlayerCollision script
    public void ResetSpeed(float penalty)
    {
        currentForwardSpeed = forwardSpeed * penalty;
    }

    void UpdateTargetLaneX()
    {
        targetLaneX = (currentLane - 1) * laneWidth;
    }

    void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }
}