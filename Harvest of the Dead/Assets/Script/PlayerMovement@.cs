using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float gravity = -9.81f;
    public bool isDriving = false;

    private CharacterController characterController;
    private Animator animator;
    private Vector3 verticalVelocity;

    private bool isInCarRange = false;
    private CarController currentCar;
    //-------------------------------------------------2nd-------------------
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Handle getting in and out of the car
        if (isInCarRange && Input.GetKeyDown(KeyCode.G))
        {
            if (currentCar != null)
            {
                currentCar.EnableDriving(this.gameObject);
                this.enabled = false; // Disable this script
            }
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            if (currentCar != null)
            {
                currentCar.DisableDriving();
                this.enabled = true; // Re-enable this script
            }
        }

        // The rest of the movement code only runs if the script is enabled
        if (!enabled) return;

        bool isMovingForward = Input.GetKey(KeyCode.W);
        bool isMovingBack = Input.GetKey(KeyCode.S);

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

        if (characterController.isGrounded)
        {
            verticalVelocity.y = -0.5f;
        }
        else
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }

        Vector3 finalMove = moveDirection + verticalVelocity;
        characterController.Move(finalMove * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isHoldingGun", true);
            animator.SetTrigger("Shoot");
        }

        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("isHoldingGun", false);
        }
    }


    
    void OnTriggerEnter(Collider other)
    {
        CarController car = other.GetComponent<CarController>();
        if (car != null)
        {
            isInCarRange = true;
            currentCar = car;
            Debug.Log("Press 'G' to get in the car.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CarController>() != null)
        {
            isInCarRange = false;
            currentCar = null;
        }
    }
}