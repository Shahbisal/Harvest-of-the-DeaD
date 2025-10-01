using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public Transform leftAttachPoint; // Assign in Inspector (drag the player's LeftAttachPoint here)

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GunRotate rotateScript = GetComponent<GunRotate>();
            if (rotateScript != null && leftAttachPoint != null)
            {
                rotateScript.StopRotationAndAttach(leftAttachPoint);
                Destroy(gameObject, 30f);
            }
        }
    }
}
