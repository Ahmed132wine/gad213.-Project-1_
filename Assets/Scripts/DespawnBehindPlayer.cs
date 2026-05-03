using UnityEngine;

public class DespawnBehindPlayer : MonoBehaviour
{
    [Header("Despawn Settings")]
    [Tooltip("How far behind the player before this object is destroyed")]
    public float distanceBehind = 30f;

    private Transform player;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }

        // We check every 1 second instead of in Update() 
        // This is a massive performance boost for endless runners!
        InvokeRepeating(nameof(CheckDespawn), 1f, 1f);
    }

    void CheckDespawn()
    {
        if (player == null) return;

        // If this object's Z position is less than the player's Z position minus the buffer distance
        if (transform.position.z < player.position.z - distanceBehind)
        {
            Destroy(gameObject);
        }
    }
}
