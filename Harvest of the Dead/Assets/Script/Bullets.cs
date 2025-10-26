using UnityEngine;

public class Bullets : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float lifeTime = 5f; // bullet disappears after this many seconds
    public GameObject hitEffect; // optional hit effect prefab

    // DAMAGE VALUE: Set to 100 for a one-shot kill (maxHealth = 100)
    private const int OneShotDamage = 100;

    void Start()
    {
        // Auto-destroy the bullet after some time (cleanup)
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 1. DAMAGE APPLICATION (Look up the hierarchy for the main ZombieAI script)
        // Check for the ZombieAI script on the hit object OR its parent (for hitting a limb/collider).
        ZombieAI zombie = collision.gameObject.GetComponentInParent<ZombieAI>();

        if (zombie != null)
        {
            // Apply one-shot damage (100)
            zombie.TakeDamage(OneShotDamage);
        }

        // 2. Prevent Player Damage
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        // 3. Spawn a hit effect
        if (hitEffect != null)
        {
            Instantiate(hitEffect, collision.contacts[0].point, Quaternion.identity);
        }

        // 4. Destroy the bullet IMMEDIATELY
        Destroy(gameObject);
    }
}
//---------------------------------------------------------final0