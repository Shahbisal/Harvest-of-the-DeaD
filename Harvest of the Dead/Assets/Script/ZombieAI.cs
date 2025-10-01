using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public Transform player; // Assign your player here
    public float attackRange = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isPlayerInAttackRange = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // If the player is in attack range, don't move.
        if (isPlayerInAttackRange)
        {
            agent.isStopped = true;
            // Trigger attack animation
            animator.SetBool("isAttacking", true);
        }
        else
        {
            // If the player is not in attack range, follow them.
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("isAttacking", false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the other object is the player
        if (other.CompareTag("Player"))
        {
            isPlayerInAttackRange = true;
            Debug.Log("Player is in attack range!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player has left the attack range
        if (other.CompareTag("Player"))
        {
            isPlayerInAttackRange = false;
            Debug.Log("Player has left attack range.");
        }
    }
}