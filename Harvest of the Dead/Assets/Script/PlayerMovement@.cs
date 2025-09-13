using UnityEngine;



public class PlayerController : MonoBehaviour

{

    // Public variables for movement speed and gravity

    public float moveSpeed = 10f;

    public float gravity = -9.81f;



    // Private variables to hold component references

    private CharacterController characterController;

    private Animator animator;

    private Vector3 verticalVelocity;



    void Start()

    {

        // Get the attached components

        characterController = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();



        // Lock the cursor to the center of the screen for a better gaming experience

        Cursor.lockState = CursorLockMode.Locked;

    }



    void Update()

    {

        // Get input for forward and backward movement

        bool isMovingForward = Input.GetKey(KeyCode.W);

        bool isMovingBack = Input.GetKey(KeyCode.S);



        // Update the Animator's boolean parameters for movement

        animator.SetBool("isMovingForward", isMovingForward);

        animator.SetBool("isMovingBack", isMovingBack);



        Vector3 moveDirection = Vector3.zero;

        if (isMovingForward)

        {

            moveDirection = transform.forward * moveSpeed;

        }

        else if (isMovingBack)

        {

            moveDirection = -transform.forward * moveSpeed;

        }



        // Apply gravity to keep the character on the ground

        if (characterController.isGrounded)

        {

            verticalVelocity.y = -0.5f;

        }

        else

        {

            verticalVelocity.y += gravity * Time.deltaTime;

        }



        // Combine horizontal movement and vertical velocity

        Vector3 finalMove = moveDirection + verticalVelocity;



        // Move the character using the Character Controller

        characterController.Move(finalMove * Time.deltaTime);



        // Handle mouse input for shooting and changing states

        if (Input.GetMouseButtonDown(0)) // Left mouse button

        {

            // Transition to the gun state and trigger the shoot animation

            animator.SetBool("isHoldingGun", true);

            animator.SetTrigger("Shoot");

        }



        if (Input.GetMouseButtonDown(1)) // Right mouse button

        {

            // Transition back to the idle state

            animator.SetBool("isHoldingGun", false);

        }

    }

}