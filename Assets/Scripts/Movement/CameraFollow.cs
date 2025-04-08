using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float camSpeed = 1.5f;   
    public Transform target;          
    public bool followPlayer = false; 
    void FixedUpdate()
    {
        if (followPlayer && target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10f);
            transform.position = Vector3.Slerp(transform.position, targetPosition, camSpeed * Time.deltaTime);
        }
    }

    public void StartFollowing()
    {
        followPlayer = true;
    }


    public void StopFollowing()
    {
        followPlayer = false;
    }
}
