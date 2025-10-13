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

        // Initialize rotationX to a value within the new clamp range
        rotationX = 0f;
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

        // FIX: Clamp the rotation between your new limits: -2.9 (up) and 4 (down)
        // NOTE: Negative values are "Up" in Unity's local X-rotation
        rotationX = Mathf.Clamp(rotationX, -2.9f, 4f);

        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}