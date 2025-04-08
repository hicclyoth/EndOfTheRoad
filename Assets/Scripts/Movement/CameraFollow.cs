using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float camSpeed = 1.5f;      // Camera movement speed
    public Transform target;           // The target (player) the camera will follow
    public bool followPlayer = false;  // Flag to control whether the camera should follow the player

    void FixedUpdate()
    {
        // If followPlayer is true, move the camera
        if (followPlayer && target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10f);
            transform.position = Vector3.Slerp(transform.position, targetPosition, camSpeed * Time.deltaTime);
        }
    }

    // Method to start camera follow (to be called from ProximityDetector or any other trigger)
    public void StartFollowing()
    {
        followPlayer = true;
    }

    // Method to stop camera follow
    public void StopFollowing()
    {
        followPlayer = false;
    }
}
