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

    [Header("Gun Settings")]
    public GameObject bulletPrefab;     // Assign your bullet prefab here
    public Transform gunPoint;          // Assign the gun tip here
    public float fireCooldown = 0.2f;   // Firing delay
    private float nextFireTime = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Handle getting in and out of car
        if (isInCarRange && Input.GetKeyDown(KeyCode.G))
        {
            if (currentCar != null)
            {
                currentCar.EnableDriving(this.gameObject);
                this.enabled = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            if (currentCar != null)
            {
                currentCar.DisableDriving();
                this.enabled = true;
            }
        }

        if (!enabled) return;

        // Movement inputs
        bool isMovingForward = Input.GetKey(KeyCode.W);
        bool isMovingBack = Input.GetKey(KeyCode.S);
        bool isMovingLeft = Input.GetKey(KeyCode.A);
        bool isMovingRight = Input.GetKey(KeyCode.D);

        // --- ANIMATION PARAMETER UPDATES ---
        animator.SetBool("isMovingForward", isMovingForward);
        animator.SetBool("isMovingBack", isMovingBack);

        // FIX for stuck running animation: Determine if the player is moving at all.
        bool isMoving = isMovingForward || isMovingBack || isMovingLeft || isMovingRight;
        animator.SetBool("isRunning", isMoving);
        // ------------------------------------

        Vector3 moveDirection = Vector3.zero;
        if (isMovingForward)
        {
            moveDirection = transform.forward * moveSpeed;
        }
        else if (isMovingBack)
        {
            moveDirection = -transform.forward * moveSpeed;
        }

        // Combined movement for WA, WD, SA, SD
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            moveDirection = (-transform.right + transform.forward).normalized * moveSpeed;
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            moveDirection = (transform.right + transform.forward).normalized * moveSpeed;
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            moveDirection = (-transform.right - transform.forward).normalized * moveSpeed;
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            moveDirection = (transform.right - transform.forward).normalized * moveSpeed;
        }

        if (characterController.isGrounded)
            verticalVelocity.y = -0.5f;
        else
            verticalVelocity.y += gravity * Time.deltaTime;

        Vector3 finalMove = moveDirection + verticalVelocity;
        characterController.Move(finalMove * Time.deltaTime);

        // Shooting logic
        if (Input.GetMouseButtonDown(0) && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;

            // Player is now holding/readying the gun
            animator.SetBool("isHoldingGun", true);

            animator.SetTrigger("Shoot");
            Shoot();
        }

        // FIX: Logic for putting the gun away (transitioning back to root|Idle_Menu)
        // We use the Right Mouse Button (1) to signal the gun is being put away.
        if (Input.GetMouseButtonDown(1))
        {
            // This parameter must be set to false to trigger the transition from 
            // root|Idle_Gun back to root|Idle_Menu in the Animator.
            animator.SetBool("isHoldingGun", false);
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && gunPoint != null)
        {
            Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
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