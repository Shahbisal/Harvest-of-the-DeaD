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