using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewWithMouseWheel : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 1.0f;

    private void Update()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollWheel) > 0.0f)
        {
            float scrollAmount = scrollWheel * scrollSpeed;
            Vector2 newPosition = scrollRect.normalizedPosition;
            newPosition.y += scrollAmount;
            newPosition.y = Mathf.Clamp01(newPosition.y);
            scrollRect.normalizedPosition = newPosition;
            Debug.Log(newPosition + " Scroll Value");
        }
    }
}
