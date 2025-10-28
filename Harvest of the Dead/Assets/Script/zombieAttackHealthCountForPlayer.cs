using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public float attackDamage = 10f;
    public float attackRange = 2f;
    public float attackRate = 1f; // attacks per second
    private float nextAttackTime = 0f;

    private Transform player;
    private PlayerHealth playerHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (player == null) return;

        // Distance between zombie and player
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void Attack()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("Zombie attacked player!");
        }
    }
}
