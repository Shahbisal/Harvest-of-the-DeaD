using UnityEngine;

public class Bullets : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float lifeTime = 5f; // bullet disappears after this many seconds
    public GameObject hitEffect; // optional hit effect prefab

    void Start()
    {
        // Auto-destroy the bullet after some time (cleanup)
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // NEW CODE: Check if the bullet hits a Zombie and deal damage.
        ZombieAI zombie = collision.gameObject.GetComponent<ZombieAI>();
        if (zombie != null)
        {
            // Damage value set to 100 to ensure ONE hit kill (100 >= 100 max health).
            // ZombieAI maxHealth is 100, so 100 damage is a one-shot kill.
            zombie.TakeDamage(100);
        }

        // Prevents the bullet from destroying itself when it hits the "Player" tagged object.
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        // Spawn a hit effect if assigned
        if (hitEffect != null)
        {
            Instantiate(hitEffect, collision.contacts[0].point, Quaternion.identity);
        }

        // Destroy the bullet IMMEDIATELY when it hits a non-player object.
        Destroy(gameObject);
    }
}