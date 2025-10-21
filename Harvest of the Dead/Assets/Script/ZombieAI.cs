using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public Transform player;            // Assign your player here
    public float detectionRange = 10f;  // Zombie becomes active when player is within this distance
    public float attackRange = 2f;      // Distance to stop and start attacking
    public float rotationSpeed = 5f;    // Speed at which the zombie turns to face the player

    [Header("Zombie Stats")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Movement Settings")]
    public float zombieSpeed = 3.5f; // 👈 Editable in Inspector

    private NavMeshAgent agent;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (agent != null)
        {
            // Start coroutine to safely initialize the NavMeshAgent speed
            StartCoroutine(InitializeAgent());
        }
    }

    // --- Initialize NavMeshAgent safely ---
    private System.Collections.IEnumerator InitializeAgent()
    {
        // Wait one frame so NavMeshAgent initializes properly on the NavMesh
        yield return null;

        if (agent == null) yield break;

        agent.speed = zombieSpeed;  // 👈 Apply the Inspector speed here
        agent.isStopped = true;     // Keep zombie idle initially
    }

    void Update()
    {
        if (isDead || player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool isPlayerDetected = distanceToPlayer <= detectionRange;
        bool isPlayerInAttackRange = distanceToPlayer <= attackRange;

        if (isPlayerDetected)
        {
            if (isPlayerInAttackRange)
            {
                // --- ATTACK STATE ---
                agent.isStopped = true;

                // Rotate smoothly toward the player
                Vector3 direction = (player.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

                animator.SetBool("isAttacking", true);
                animator.SetFloat("Speed", 0f);
            }
            else
            {
                // --- CHASE STATE ---
                agent.isStopped = false;
                agent.SetDestination(player.position);
                animator.SetBool("isAttacking", false);

                float currentSpeed = agent.velocity.magnitude;
                animator.SetFloat("Speed", currentSpeed);
            }
        }
        else
        {
            // --- IDLE STATE ---
            agent.isStopped = true;
            agent.ResetPath();

            animator.SetBool("isAttacking", false);
            animator.SetFloat("Speed", 0f);
        }
    }

    // --- DAMAGE & DEATH HANDLING ---

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        animator.SetBool("isDead", true);
        animator.SetBool("isAttacking", false);
        animator.SetFloat("Speed", 0f);

        // Optional: Destroy(gameObject, 5f);
    }
}
