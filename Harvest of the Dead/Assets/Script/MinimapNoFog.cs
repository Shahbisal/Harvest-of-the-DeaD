using UnityEngine;
using UnityEngine.Rendering; // Required for RenderPipelineManager in URP/HDRP

public class MinimapNoFog : MonoBehaviour
{
    private bool initialFogState;

    // Called right before the camera starts rendering
    private void OnPreRender()
    {
        // Save the current global fog state
        initialFogState = RenderSettings.fog;

        // Disable global fog for this render pass
        RenderSettings.fog = false;
    }

    // Called right after the camera finishes rendering
    private void OnPostRender()
    {
        // Restore the global fog state
        RenderSettings.fog = initialFogState;
    }
}