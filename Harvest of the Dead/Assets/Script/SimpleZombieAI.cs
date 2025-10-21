using UnityEngine;

public class SimpleZombieAI_NoNavMesh : MonoBehaviour
{
    // 
    // --- ACCESSIBLE IN INSPECTOR ---
    //
    [Header("Target & Movement")]
    // Drag your Player GameObject (Transform) here in the Inspector!
    public Transform target;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float walkSpeed = 1.5f;
    public float chaseSpeed = 3.5f;

    [Header("Attack Settings")]
    public float attackCooldown = 2f; // Time between attacks

    // --- PRIVATE VARIABLES ---
    private float timeSinceLastAttack;
    private float currentSpeed;

    // --- STATE ENUM ---
    public enum ZombieState { IDLE, CHASE, ATTACK }
    private ZombieState currentState = ZombieState.IDLE;

    void Start()
    {
        // Safety check to ensure the target is assigned
        if (target == null)
        {
            Debug.LogError("The 'Target' (Player) is not assigned in the Inspector for " + gameObject.name + "!");
            enabled = false;
        }
        timeSinceLastAttack = attackCooldown;
    }

    void Update()
    {
        // Stop execution if there is no target
        if (target == null) return;

        // 1. STATE TRANSITIONS
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Determine the new state based on proximity
        if (distanceToTarget <= attackRange)
        {
            currentState = ZombieState.ATTACK;
        }
        else if (distanceToTarget <= chaseRange)
        {
            currentState = ZombieState.CHASE;
        }
        else
        {
            currentState = ZombieState.IDLE;
        }

        // 2. STATE EXECUTION
        switch (currentState)
        {
            case ZombieState.IDLE:
                HandleIdle();
                break;
            case ZombieState.CHASE:
                HandleChase();
                break;
            case ZombieState.ATTACK:
                HandleAttack();
                break;
        }

        // Update attack timer
        timeSinceLastAttack += Time.deltaTime;
    }

    // --- BEHAVIOR METHODS ---

    void HandleIdle()
    {
        // When idling, the zombie's speed is low, but it stays put.
        currentSpeed = walkSpeed * 0.5f;
    }

    void HandleChase()
    {
        currentSpeed = chaseSpeed;

        // Movement: Move directly toward the target (will not avoid obstacles)
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            currentSpeed * Time.deltaTime
        );

        // Rotation: Turn to look at the target while moving
        Vector3 lookDir = target.position - transform.position;
        lookDir.y = 0;

        if (lookDir != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
        }
    }

    void HandleAttack()
    {
        // Stop movement is implicit as MoveTowards is only in HandleChase

        // Rotation: Ensure the zombie faces the target before attacking
        Vector3 lookDir = target.position - transform.position;
        lookDir.y = 0;

        // **ERROR FIX:** Only rotate if the direction vector is not zero.
        if (lookDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        if (timeSinceLastAttack >= attackCooldown)
        {
            Debug.Log("Zombie ATTACKS " + target.name + "!");
            // TODO: Implement your player damage logic here
            timeSinceLastAttack = 0f;
        }
    }
}