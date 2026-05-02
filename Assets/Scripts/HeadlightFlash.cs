using UnityEngine;
using System.Collections;

public class HeadlightFlash : MonoBehaviour
{
    [Header("Settings")]
    public float flashRange = 5f;
    public float flashCooldown = 0.3f;
    public Light headlight;
    public float flashIntensity = 3f;
    public float flashDuration = 0.1f;
    public AudioSource honkSound;

    private float nextFlashTime = 0f;
    private float originalIntensity;

    void Start()
    {
        if (headlight != null)
        {
            originalIntensity = headlight.intensity;
            headlight.intensity = 0f;
        }
        if (honkSound == null) honkSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F)) && Time.time >= nextFlashTime)
        {
            StartCoroutine(FlashSequence());
        }
    }

    IEnumerator FlashSequence()
    {
        nextFlashTime = Time.time + flashCooldown;

        if (headlight != null) headlight.intensity = flashIntensity;
        if (honkSound != null && honkSound.clip != null)
            honkSound.PlayOneShot(honkSound.clip);

        AffectNPCInFront();
        ScareBirds();

        yield return new WaitForSeconds(flashDuration);
        if (headlight != null) headlight.intensity = originalIntensity;
    }

    void AffectNPCInFront()
    {
        
        Vector3 rayOrigin = transform.position + Vector3.up * 0.8f;

        
        RaycastHit hit;
        if (Physics.SphereCast(rayOrigin, 1.5f, transform.forward, out hit, flashRange))
        {
            if (hit.collider.CompareTag("NPC"))
            {
                NPCCar npc = hit.collider.GetComponent<NPCCar>();
                if (npc != null)
                {
                    npc.MoveOutOfTheWay();
                    Debug.Log("Headlight hit NPC: " + hit.collider.name);
                }
            }
        }
    }

    void ScareBirds()
    {
        // Increased radius to find birds hanging on the back
        Collider[] hits = Physics.OverlapSphere(transform.position, flashRange + 5f);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Bird"))
            {
                AngryBird bird = hit.GetComponent<AngryBird>();
                if (bird != null)
                {
                    bird.GetScared();
                }
            }
        }
    }
}
