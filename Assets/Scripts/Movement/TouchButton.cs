using UnityEngine;
using UnityEngine.UI;

public class TouchButton : MonoBehaviour
{
    public bool IsPressed { get; private set; }

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        IsPressed = false;

        foreach (Touch touch in Input.touches)
        {

            //Getting the coordinates of the touch
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                touch.position,
                null,
                out localPoint
            );
            //Checking if the touch was inside the rectTransform(Button)
            if (rectTransform.rect.Contains(localPoint))
            {
                IsPressed = true;
                break;
            }
        }

#if UNITY_EDITOR
        // Mouse test support in Editor
        Vector2 mousePos = Input.mousePosition;
        Vector2 localMouse;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePos, null, out localMouse);
        if (rectTransform.rect.Contains(localMouse) && Input.GetMouseButton(0))
        {
            IsPressed = true;
        }
#endif
    }
}
