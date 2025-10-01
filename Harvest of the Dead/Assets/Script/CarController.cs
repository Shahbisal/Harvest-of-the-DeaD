using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float turnSpeed = 100f;
    public Transform playerAttachPoint;
    public Transform playerExitPoint;

    private bool isBeingDriven = false;
    private GameObject playerObject;

    void Update()
    {
        if (!isBeingDriven) return;

        // Get keyboard input
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // Move the car forward/backward
        Vector3 moveDirection = transform.forward * moveInput * moveSpeed * Time.deltaTime;
        transform.Translate(moveDirection, Space.World);

        // Rotate the car left/right
        transform.Rotate(Vector3.up, turnInput * turnSpeed * Time.deltaTime);
    }

    public void EnableDriving(GameObject player)
    {
        isBeingDriven = true;
        playerObject = player;

        // Attach the player to the car
        player.transform.SetParent(playerAttachPoint);
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;

        // Disable player's CharacterController to prevent movement
        player.GetComponent<CharacterController>().enabled = false;

        // Tell the PlayerController script that it's now driving
        PlayerController playerScript = player.GetComponent<PlayerController>();
        if (playerScript != null)
        {
            playerScript.isDriving = true;
        }
    }

    public void DisableDriving()
    {
        // Add this check to prevent the error
        if (playerObject == null) return;

        isBeingDriven = false;

        // Re-enable player's CharacterController
        playerObject.GetComponent<CharacterController>().enabled = true;

        // Tell the PlayerController script that it's no longer driving
        PlayerController playerScript = playerObject.GetComponent<PlayerController>();
        if (playerScript != null)
        {
            playerScript.isDriving = false;
        }

        // Detach and move the player to the exit point
        playerObject.transform.position = playerExitPoint.position;
        playerObject.transform.rotation = playerExitPoint.rotation;
        playerObject.transform.SetParent(null);
    }
}