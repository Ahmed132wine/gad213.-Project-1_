using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -7);
    public float smoothSpeed = 10f;
    public float lookAtHeightOffset = 1.2f;

    // --- Shake Variables ---
    public float currentShakeDuration = 0f;
    public float currentShakeMagnitude = 0f;

    void Start()
    {
        if (target == null) return;
        transform.position = target.position + offset;
        transform.LookAt(target.position + Vector3.up * lookAtHeightOffset);
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // 1. Calculate the normal smooth follow position
        Vector3 desiredPosition = target.position + offset;
        Vector3 basePosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);

        // 2. Apply Screen Shake if active
        if (currentShakeDuration > 0)
        {
            // Add a random offset inside a sphere to create the violent shake
            basePosition += Random.insideUnitSphere * currentShakeMagnitude;

            // Reduce the timer
            currentShakeDuration -= Time.fixedDeltaTime;
        }

        // Apply final position
        transform.position = basePosition;

        // Keep looking at player
        transform.LookAt(target.position + Vector3.up * lookAtHeightOffset);
    }

    
    public void TriggerShake(float duration, float magnitude)
    {
        currentShakeDuration = duration;
        currentShakeMagnitude = magnitude;
    }
}