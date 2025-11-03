using UnityEngine;
using UnityEngine.UI;

public class HorizontalMouseScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 10f;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            // Move horizontally instead of vertically
            scrollRect.horizontalNormalizedPosition -= scroll * scrollSpeed * Time.deltaTime;
        }
    }
}
