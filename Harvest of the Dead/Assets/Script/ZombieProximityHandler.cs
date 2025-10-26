using UnityEngine;

public class ZombieProximityHandler : MonoBehaviour
{
    private AudioSource audioSource;
    private Transform playerTransform;
    private Animator animator;

    // Flag to stop the script's logic when the zombie dies
    private bool isShuttingDown = false;

    [Header("Behavior Settings")]
    public float detectionRange = 15f;
    public string attackAnimationBool = "IsAttacking"; // Removed from use

    [Header("Audio Settings")]
    public float maxVolume = 0.8f;
    public float volumeFadeRate = 1.0f;

    private bool playerIsInRange = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        // Find the player's transform (using modern method)
        CameraController cameraController = FindFirstObjectByType<CameraController>();

        if (cameraController != null)
        {
            playerTransform = cameraController.playerBody;
        }
        else
        {
            Debug.LogError("Player body/transform not found. Cannot track proximity.");
            enabled = false;
            return;
        }

        // --- INITIAL CHECK LOGIC ---
        if (audioSource != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            playerIsInRange = distanceToPlayer <= detectionRange;

            if (playerIsInRange)
            {
                audioSource.volume = maxVolume;
                audioSource.Play();
            }
            else
            {
                audioSource.volume = 0f;
            }
        }
    }

    void Update()
    {
        // If the shutdown flag is active, stop all logic.
        if (isShuttingDown) return;

        if (playerTransform == null || audioSource == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // --- 1. PROXIMITY STATUS LOGIC ---
        bool currentlyInRange = distanceToPlayer <= detectionRange;

        if (currentlyInRange != playerIsInRange)
        {
            playerIsInRange = currentlyInRange;

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
        if (!playerIsInRange && audioSource.volume == 0f && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Called by the ZombieAI script upon death
    public void StopAudioOnDeath()
    {
        // Sets flag to stop Update() from running
        isShuttingDown = true;

        if (audioSource != null && audioSource.isPlaying)
        {
            // Stops the audio instantly
            audioSource.Stop();
        }
    }
}