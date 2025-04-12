using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float camSpeed = 1.5f;   // Camera follow speed
    public Transform target;        // Target the camera follows
    public bool followPlayer = false; // Whether the camera is following the player

    void FixedUpdate()
    {
        // Move the camera if following the player and target is assigned
        if (followPlayer && target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10f);
            transform.position = Vector3.Slerp(transform.position, targetPosition, camSpeed * Time.deltaTime);
        }
    }

    // Start following the target
    public void StartFollowing()
    {
        followPlayer = true;
    }

    // Stop following the target
    public void StopFollowing()
    {
        followPlayer = false;
    }
}
