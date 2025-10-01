using UnityEngine;

public class PickupItem : MonoBehaviour
{
    // Key to press for pickup
    private KeyCode pickupKey = KeyCode.F;

    private bool canPickup = false;              // True if player is in trigger
    private Transform playerAttachPoint;         // Reference to chest attach point

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered pickup zone!");
            canPickup = true;
            // Find attach point inside the player
            playerAttachPoint = other.transform.Find("FireStandAttachPoint");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left pickup zone!");
            canPickup = false;
            playerAttachPoint = null;
        }
    }

    void Update()
    {
        if (canPickup && Input.GetKeyDown(pickupKey))
        {
            Debug.Log("Picked up item!");
            // Attach fire stand to player's chest point
            transform.SetParent(playerAttachPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            // Disable collider so it doesn't keep triggering
            GetComponent<Collider>().enabled = false;
        }
    }
}
