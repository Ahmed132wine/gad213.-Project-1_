using UnityEngine;

public class NPCCar : MonoBehaviour
{
    [Header("Movement")]
    public float baseSpeed = 7f;
    public float laneSwitchSpeed = 5f;
    public float laneWidth = 2.5f;
    public float yOffset = 0.5f; 

    private int currentLane = 1;
    private float targetLaneX;
    private Rigidbody rb;
    private bool isForcedSwitching = false;
    private float forcedTargetX;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; 

        currentLane = Random.Range(0, 3);
        UpdateTargetLaneX();

        // Snap to starting lane and correct height
        transform.position = new Vector3(targetLaneX, yOffset, transform.position.z);
    }

    void Update()
    {
        // Forward movement
        transform.Translate(Vector3.forward * baseSpeed * Time.deltaTime);

        // Smooth lane movement
        float targetX = isForcedSwitching ? forcedTargetX : targetLaneX;
        Vector3 pos = transform.position;
        pos.x = Mathf.MoveTowards(pos.x, targetX, laneSwitchSpeed * Time.deltaTime);
        pos.y = yOffset; // Keep it pinned to the road height
        transform.position = pos;

        if (isForcedSwitching && Mathf.Abs(transform.position.x - forcedTargetX) < 0.1f)
        {
            isForcedSwitching = false;
            targetLaneX = forcedTargetX;
        }
    }

    public void MoveOutOfTheWay()
    {
        if (isForcedSwitching) return;

        // If in middle, go left or right. If on side, go middle.
        if (currentLane == 1)
            currentLane = (Random.value > 0.5f) ? 0 : 2;
        else
            currentLane = 1;

        UpdateTargetLaneX();
        forcedTargetX = targetLaneX;
        isForcedSwitching = true;

        StartCoroutine(FlashRed());
    }

    void UpdateTargetLaneX()
    {
        targetLaneX = (currentLane - 1) * laneWidth;
    }

    System.Collections.IEnumerator FlashRed()
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            rend.material.color = Color.red;
            yield return new WaitForSeconds(0.3f);
            rend.material.color = Color.white;
        }
    }
}
