using UnityEngine;

public class BackpackPickup : MonoBehaviour
{
    public KeyCode pickupKey = KeyCode.F;
    private bool canPickup = false;
    private Transform playerBagAttachPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
            // Find the BagAttach point inside the Player
            playerBagAttachPoint = other.transform.Find("BagAttach");
        }
    }
    //----------------------------final---------------------------------------------
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
            playerBagAttachPoint = null;
        }
    }

    void Update()
    {
        if (canPickup && Input.GetKeyDown(pickupKey))
        {
            // Attach the backpack to the player's BagAttach point
            transform.SetParent(playerBagAttachPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            // Disable the backpack's collider after pickup
            GetComponent<Collider>().enabled = false;
        }
    }
}