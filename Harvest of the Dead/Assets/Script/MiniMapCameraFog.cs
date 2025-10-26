using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class MiniMapCameraFogURP : MonoBehaviour
{
    private Volume globalVolume;
    private LayerMask originalMask;

    void Start()
    {
        // Find the main/global Volume in your scene
        globalVolume = Object.FindFirstObjectByType<Volume>();

        // Store original layer mask
        var cam = GetComponent<Camera>();
        originalMask = cam.cullingMask;
    }

    void OnPreCull()
    {
        // Disable volume effects (like fog) by clearing volume layer mask
        var camData = GetComponent<UniversalAdditionalCameraData>();
        if (camData != null)
        {
            camData.volumeLayerMask = 0; // No volume effects
        }
    }

    void OnPostRender()
    {
        // Restore volume layer mask if needed (optional)
        var camData = GetComponent<UniversalAdditionalCameraData>();
        if (camData != null)
        {
            camData.volumeLayerMask = ~0; // Restore all layers
        }
    }
}
