using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float camSpeed = 5f;   
    public Transform target;
    public bool followPlayer = false; 
    public Vector2 offset;

    void LateUpdate()
    {
        if (followPlayer && target != null)
        {
            Vector3 targetPosition = new Vector3(
                target.position.x + offset.x,
                target.position.y + offset.y,
                -10f
            );

            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                camSpeed * Time.deltaTime
            );
        }
    }

    public void StartFollowing() => followPlayer = true;

    public void StopFollowing() => followPlayer = false;
}
