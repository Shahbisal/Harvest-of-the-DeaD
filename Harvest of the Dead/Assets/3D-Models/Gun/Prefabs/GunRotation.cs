using UnityEngine;

public class GunRotate : MonoBehaviour
{
    public float rotateSpeed = 2f; // Degrees per second
    public bool isAttached = false; // If true, stop rotating
    public Vector3 attachOffset = new Vector3(-0.5f, 0, 0); // Position offset to the left

    void Update()
    {
        if (!isAttached) // Only rotate if not attached
        {
            transform.Rotate(0, rotateSpeed, 0);
        }
    }

    // Call this when attaching to player
    public void StopRotationAndAttach(Transform attachPoint)
    {
        isAttached = true; // Stops rotation

        // Parent gun to attach point
        transform.SetParent(attachPoint);

        // Offset the gun position relative to attach point
        transform.localPosition = attachOffset;
        transform.localRotation = Quaternion.identity;

        // Disable physics so it doesn't move
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }

}
