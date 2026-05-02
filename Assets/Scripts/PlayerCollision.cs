using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private LaneMovement3D movement;
    private FoodMeter foodMeter;

    void Start()
    {
        movement = GetComponent<LaneMovement3D>();
        foodMeter = GetComponent<FoodMeter>();
    }

    private void OnCollisionEnter(Collision hit)
    {
        // Debug.Log ensures you can see in the Console if ANY collision happens
        Debug.Log("Collision detected with: " + hit.gameObject.name);

        if (hit.gameObject.CompareTag("NPC"))
        {
            Debug.Log("Hit an NPC car!");

            if (movement != null)
                movement.ResetSpeed(0.4f); // Slow to 40% speed

            if (foodMeter != null)
                foodMeter.LoseFood(10);
        }
    }
}
