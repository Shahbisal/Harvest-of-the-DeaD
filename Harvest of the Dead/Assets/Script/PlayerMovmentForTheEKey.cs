using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Public variable for the running speed
    public float runSpeed = 10f;
    public float gravity = -9.81f;

    // Private variables to hold component references
    private CharacterController characterController;
    private Animator animator;
    private Vector3 verticalVelocity;

    void Start()
    {
        // Get the CharacterController and Animator components
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the 'E' key is being pressed
        bool isRunning = Input.GetKey(KeyCode.E);

        // Tell the Animator to play the running animation when 'E' is pressed
        if (animator != null)
        {
            animator.SetBool("isRunning", isRunning);
        }

        Vector3 moveDirection = Vector3.zero;

        // If the 'E' key is pressed, move the character forward at runSpeed
        if (isRunning)
        {
            moveDirection = transform.forward * runSpeed;
        }

        // Apply gravity
        if (characterController != null && characterController.isGrounded)
        {
            verticalVelocity.y = -0.5f;
        }
        else if (characterController != null)
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }

        // Combine movement and gravity
        Vector3 finalMove = moveDirection + verticalVelocity;

        // Move the character
        if (characterController != null)
        {
            characterController.Move(finalMove * Time.deltaTime);
        }
    }
}