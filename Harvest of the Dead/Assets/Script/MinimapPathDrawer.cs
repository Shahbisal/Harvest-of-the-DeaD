using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    public Transform player;
    public Transform goalDestination;
    private LineRenderer lineRenderer;

    void Start()
    {
        // Get or add a LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Line thickness
        lineRenderer.startWidth = 1.0f;   // Thicker start
        lineRenderer.endWidth = 1.0f;     // Thicker end
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;

        // Line color and material
        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = Color.yellow;
        lineRenderer.material = mat;

        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
    }

    void Update()
    {
        if (player == null || goalDestination == null)
            return;

        Vector3 startPos = player.position;
        Vector3 endPos = goalDestination.position;

        // Keep slightly above terrain height
        if (Physics.Raycast(player.position + Vector3.up * 100, Vector3.down, out RaycastHit hit1))
            startPos.y = hit1.point.y + 1.0f;

        if (Physics.Raycast(goalDestination.position + Vector3.up * 100, Vector3.down, out RaycastHit hit2))
            endPos.y = hit2.point.y + 1.0f;

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
}
