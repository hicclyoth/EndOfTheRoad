using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerMovement playerMovement;  
    public bool isLeftButton;  
    public bool isRightButton;  
    public bool isJumpButton;   

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLeftButton)
            playerMovement.MoveLeft();
        else if (isRightButton)
            playerMovement.MoveRight();
        else if (isJumpButton)
            playerMovement.JumpPress();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isLeftButton || isRightButton)
            playerMovement.StopMovement();
        else if (isJumpButton)
            playerMovement.JumpRelease();
    }
}
