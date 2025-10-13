using UnityEngine;

public class Gun : MonoBehaviour
{
    // NOTE: The animation state name below MUST match the Animator.
    private const string IdleStateName = "root|Idle_Menu";

    public Transform bulletSpawnPoint;   // Where bullet spawns
    public GameObject bulletPrefab;      // Bullet prefab
    public float bulletSpeed = 10f;      // Bullet speed
    public Animator gunAnimator;         // Gun Animator

    void Update()
    {
        // Fire when LEFT mouse button (0) is pressed
        if (Input.GetMouseButtonDown(0))
        {
            // FIX 1: Fire only if the gun is NOT in the correct Idle state.
            if (gunAnimator != null && !IsInIdleState())
            {
                FireBullet();
            }
            else if (gunAnimator == null)
            {
                // Safety check: allow fire if no animator is present
                FireBullet();
            }
            else
            {
                Debug.Log("Cannot shoot while in the Idle Menu state!");
            }
        }
    }

    // Checks the current animation state against the constant IdleStateName
    bool IsInIdleState()
    {
        // Check both the Idle Menu and Idle Focus states as they are both idle-like
        var currentState = gunAnimator.GetCurrentAnimatorStateInfo(0);
        return currentState.IsName(IdleStateName) || currentState.IsName("root|Idle_Focus");
    }

    void FireBullet()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null)
        {
            Debug.LogWarning("Bullet prefab or spawn point not assigned!");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // FIX 2: Reverting to .forward. You MUST now adjust the bulletSpawnPoint rotation in the Editor.
            rb.linearVelocity = bulletSpawnPoint.forward * bulletSpeed;
        }

        if (gunAnimator != null)
        {
            gunAnimator.SetTrigger("Shoot"); // trigger your shoot animation
        }
    }
}