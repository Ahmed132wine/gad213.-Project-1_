using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -7);
    public float smoothSpeed = 10f;
    public float lookAtHeightOffset = 1.2f;

    void Start()
    {
        if (target == null) return;

        // FIX THE STARTUP OFFSET: 
        // Force the camera to the exact position on Frame 1 
        // so you don't have to "rectify" it by pausing.
        transform.position = target.position + offset;
        transform.LookAt(target.position + Vector3.up * lookAtHeightOffset);
    }

    // FIX THE JITTER:
    // We use FixedUpdate because your LaneMovement3D uses FixedUpdate.
    // This keeps the camera and the player on the exact same "clock."
    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Use Time.fixedDeltaTime to match the physics clock
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);

        // Keep the player centered in the frame
        transform.LookAt(target.position + Vector3.up * lookAtHeightOffset);
    }
}