using UnityEngine;
using System.Collections;

public class BoostSystem : MonoBehaviour
{
    [Header("Boost Settings")]
    public float boostMultiplier = 1.8f;
    public float boostDuration = 2f;
    public float boostCooldown = 3f;

    [Header("Visual Feedback")]
    public ParticleSystem boostParticles;
    public TrailRenderer boostTrail;

    private LaneMovement3D movement;
    private float originalSpeed;
    private bool isBoosting = false;
    private float nextBoostTime = 0f;

    void Start()
    {
        // Find the movement script
        movement = GetComponent<LaneMovement3D>();
        if (movement == null)
        {
            Debug.LogError("BoostSystem: No LaneMovement3D found on this GameObject!");
        }
        else
        {
            originalSpeed = movement.forwardSpeed;
            Debug.Log("BoostSystem initialized. Original speed: " + originalSpeed);
        }

        // Disable trail at start if it exists
        if (boostTrail != null)
        {
            boostTrail.enabled = false;
        }
    }

    void Update()
    {
        // Press Left Shift to boost
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isBoosting && Time.time >= nextBoostTime && movement != null)
        {
            StartCoroutine(ActivateBoost());
        }
    }

    IEnumerator ActivateBoost()
    {
        isBoosting = true;

        // Apply boost speed
        movement.forwardSpeed = originalSpeed * boostMultiplier;
        Debug.Log("BOOST ACTIVATED! Speed: " + movement.forwardSpeed);

        // Visual effects
        if (boostParticles != null)
        {
            boostParticles.Play();
        }
        if (boostTrail != null)
        {
            boostTrail.enabled = true;
        }

        // Wait for boost duration
        yield return new WaitForSeconds(boostDuration);

        // End boost
        movement.forwardSpeed = originalSpeed;
        isBoosting = false;
        nextBoostTime = Time.time + boostCooldown;

        if (boostParticles != null)
        {
            boostParticles.Stop();
        }
        if (boostTrail != null)
        {
            boostTrail.enabled = false;
        }

        Debug.Log("Boost ended. Speed back to: " + movement.forwardSpeed);
    }
}
