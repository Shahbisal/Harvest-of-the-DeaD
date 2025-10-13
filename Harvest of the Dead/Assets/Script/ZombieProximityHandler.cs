using UnityEngine;

public class ZombieProximityHandler : MonoBehaviour
{
    private AudioSource audioSource;
    private Transform playerTransform;
    private Animator animator;

    [Header("Behavior Settings")]
    public float detectionRange = 15f;
    public string attackAnimationBool = "IsAttacking";

    [Header("Audio Settings")]
    public float maxVolume = 0.8f;
    public float volumeFadeRate = 1.0f;

    private bool playerIsInRange = false;

    void Start()
    {
        // Get components
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        // Find the player's transform
        if (FindObjectOfType<CameraController>() != null)
        {
            playerTransform = FindObjectOfType<CameraController>().playerBody;
        }
        else
        {
            Debug.LogError("Player body/transform not found. Cannot track proximity.");
            enabled = false;
            return;
        }

        // --- NEW INITIAL CHECK LOGIC ---
        if (audioSource != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            playerIsInRange = distanceToPlayer <= detectionRange;

            if (playerIsInRange)
            {
                // If already in range, start the sound immediately at MAX volume 
                // and set the animator state.
                audioSource.volume = maxVolume;
                audioSource.Play();
                if (animator != null)
                {
                    animator.SetBool(attackAnimationBool, true);
                }
            }
            else
            {
                // If out of range, ensure volume is silent.
                audioSource.volume = 0f;
            }
        }
    }

    void Update()
    {
        if (playerTransform == null || audioSource == null || animator == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // --- 1. BEHAVIOR (Animation) LOGIC ---
        bool currentlyInRange = distanceToPlayer <= detectionRange;

        if (currentlyInRange != playerIsInRange)
        {
            // State change detected (entering or exiting range)
            playerIsInRange = currentlyInRange;

            // Set the Animator boolean to switch behavior (e.g., Idle to Run/Attack)
            animator.SetBool(attackAnimationBool, playerIsInRange);

            // Start the audio playback only when entering range for the first time
            if (playerIsInRange && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        // --- 2. AUDIO FADE LOGIC (Handles smooth transitions) ---
        float targetVolume = playerIsInRange ? maxVolume : 0f;

        // Smoothly move the current volume towards the target volume
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, volumeFadeRate * Time.deltaTime);

        // --- 3. STOP AUDIO WHEN FADED ---
        // If the zombie is out of range AND the volume has successfully faded to zero, stop the sound entirely.
        if (!playerIsInRange && audioSource.volume == 0f && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}