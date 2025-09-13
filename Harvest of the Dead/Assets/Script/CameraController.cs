using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 2f;
    public Transform playerBody;

    private float rotationX = 0f;

    void Start()
    {
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Rotate the player body horizontally
        playerBody.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically (looking up and down)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Prevents flipping
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}