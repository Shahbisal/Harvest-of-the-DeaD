using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public Transform player;            // Assign your player here
    public float detectionRange = 10f;  // NEW: Zombie becomes active when player is within this distance
    public float attackRange = 2f;      // Distance to stop and start attacking
    public float rotationSpeed = 5f;    // Speed at which the zombie turns to face the player

    // NEW: Variable to control the zombie's running speed
    [Header("Movement Settings")]
    public float zombieRunSpeed = 3.5f; // Control the running speed in the Inspector

    // Health variables (You will need these for the die logic)
    public int maxHealth = 100;
    private int currentHealth;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isDead = false; // Internal flag

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        // Set the NavMeshAgent speed based on the public variable
        if (agent != null)
        {
            agent.speed = zombieRunSpeed; // APPLYING THE NEW SPEED CONTROL
            agent.isStopped = true;
        }
    }

    void Update()
    {
        // --- 1. DEATH CHECK ---
        if (isDead || player == null)
        {
            return;
        }

        // --- 2. DISTANCE CALCULATIONS ---
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        bool isPlayerDetected = distanceToPlayer <= detectionRange;
        bool isPlayerInAttackRange = distanceToPlayer <= attackRange;

        if (isPlayerDetected)
        {
            // ZOMBIE IS ACTIVE (RUN OR ATTACK)

            if (isPlayerInAttackRange)
            {
                // --- ATTACK STATE ---
                agent.isStopped = true;

                // Make the zombie face the player while attacking
                Vector3 direction = (player.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

                animator.SetBool("isAttacking", true);

                // Set speed to 0 so the Movement Blend Tree plays IDLE during attack
                animator.SetFloat("Speed", 0f);
            }
            else // Detected but not in attack range: CHASE/RUN
            {
                // --- CHASE/RUN STATE ---
                agent.isStopped = false;
                agent.SetDestination(player.position);
                animator.SetBool("isAttacking", false);

                // Set speed for running animation
                float currentSpeed = agent.velocity.magnitude;
                animator.SetFloat("Speed", currentSpeed);
            }
        }
        else // Player is outside the Detection Range
        {
            // ZOMBIE IS PASSIVE (IDLE)

            // Stop movement and reset destination
            agent.isStopped = true;
            agent.ResetPath();

            // Ensure Attack is false
            animator.SetBool("isAttacking", false);

            // Set speed to 0 to ensure the Movement Blend Tree plays IDLE
            animator.SetFloat("Speed", 0f);
        }
    }

    // --- DAMAGE AND DEATH FUNCTIONS ---

    /// <summary>
    /// This method is called when the zombie takes damage.
    /// (No gun/bullet code was present, only the TakeDamage signature, which remains for interaction with your bullet script).
    /// </summary>
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

        // Stop all movement and navigation
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        // Trigger the Die animation 
        animator.SetBool("isDead", true);

        // Ensure animations are reset
        animator.SetBool("isAttacking", false);
        animator.SetFloat("Speed", 0f);

        // Example: Destroy(gameObject, 5f);
    }
}