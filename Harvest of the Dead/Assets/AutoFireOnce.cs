using UnityEngine;

public class AutoFireOnce : MonoBehaviour
{
    private Gun gunScript;
    private Animator animator;

    void Start()
    {
        // 1. Find the Gun script component using the modern Unity function
        gunScript = GetComponent<Gun>();
        if (gunScript == null)
        {
            gunScript = FindFirstObjectByType<Gun>();
        }

        // 2. Get the Animator reference
        if (gunScript != null)
        {
            animator = gunScript.gunAnimator;
        }

        if (gunScript != null && animator != null)
        {
            // --- FIRE THE SHOT ---

            // A. Set the parameter to enter the shooting stance (required for visual stability)
            animator.SetBool("isHoldingGun", true);

            // B. Trigger the shooting animation
            animator.SetTrigger("Shoot");

            // C. Fire the actual bullet
            gunScript.FireBullet();

            // --- END FIRE ---

            Debug.Log("AutoFireOnce: Initial shot fired. Script disabled.");
        }
        else
        {
            Debug.LogError("AutoFireOnce: Could not find Gun script or Animator reference to fire shot.");
        }

        // 3. Disable the script immediately after running
        this.enabled = false;
    }
}