using UnityEngine;

public class Gun : MonoBehaviour
{
    // NOTE: The animation state name below MUST match the Animator.
    private const string IdleStateName = "root|Idle_Menu";

    public Transform bulletSpawnPoint;   // Where bullet spawns
    public GameObject bulletPrefab;      // Bullet prefab
    public float bulletSpeed = 10f;      // Bullet speed
    public Animator gunAnimator;         // Gun Animator

    // New: Reference to the component to rotate for visual aiming (e.g., weapon bone)
    public Transform aimingPivot;

    // New: Limits the visual rotation, e.g., 60 degrees up, 60 degrees down
    public float verticalAimLimit = 60f;

    void Update()
    {
        // 1. VISUAL AIMING UPDATE
        HandleVisualAiming();

        // 2. FIRING INPUT
        if (Input.GetMouseButtonDown(0))
        {
            if (gunAnimator != null && !IsInIdleState())
            {
                FireBullet();
            }
            else if (gunAnimator == null)
            {
                FireBullet();
            }
            else
            {
                Debug.Log("Cannot shoot while in the Idle Menu state!");
            }
        }
    }

    void HandleVisualAiming()
    {
        if (aimingPivot == null || Camera.main == null) return;

        // Get the vertical rotation (pitch) from the main camera.
        float cameraPitch = Camera.main.transform.localEulerAngles.x;

        // Correct for Unity's 0-360 degree rotation
        if (cameraPitch > 180)
        {
            cameraPitch -= 360;
        }

        // Clamp the pitch to your desired limits
        cameraPitch = Mathf.Clamp(cameraPitch, -verticalAimLimit, verticalAimLimit);

        // Apply the pitch rotation to the pivot point (the gun or aiming bone).
        // Use the negative camera pitch to ensure the gun rotates up when the camera looks up.
        aimingPivot.localEulerAngles = new Vector3(
            -cameraPitch,
            aimingPivot.localEulerAngles.y,
            aimingPivot.localEulerAngles.z
        );
    }

    bool IsInIdleState()
    {
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
            // The FIX: Use the camera's forward vector for direction, ensuring the bullet goes where the screen is aiming.
            Vector3 aimDirection = Camera.main.transform.forward;
            rb.linearVelocity = aimDirection * bulletSpeed;
        }

        if (gunAnimator != null)
        {
            gunAnimator.SetTrigger("Shoot");
        }
    }
}