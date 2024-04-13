using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Adds the ability to scroll up and down in scroll view (Should be a thing already but had to write custom script)
/// </summary>
public class ScrollViewWithMouseWheel : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 1.0f;

    private void Update()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel"); //Gets thw scroll wheel

        if (Mathf.Abs(scrollWheel) > 0.0f) //If any movement
        {
            float scrollAmount = scrollWheel * scrollSpeed;
            Vector2 newPosition = scrollRect.normalizedPosition;
            newPosition.y += scrollAmount;
            newPosition.y = Mathf.Clamp01(newPosition.y);
            scrollRect.normalizedPosition = newPosition;
        }
    }
}
