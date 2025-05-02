using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float camSpeed = 5f;
    public Transform target;
    public bool followPlayer = false;
    public Vector2 offset;

    public BoxCollider2D bounds;
    private Vector2 minPosition;
    private Vector2 maxPosition;

    void Start()
    {
        if (bounds != null)
        {
            Bounds b = bounds.bounds;
            minPosition = b.min;
            maxPosition = b.max;
        }
    }

    void LateUpdate()
    {
        if (followPlayer && target != null)
        {
            Vector3 targetPosition = new Vector3(
                target.position.x + offset.x,
                target.position.y + offset.y,
                -10f
            );

            // Clamp to bounds if assigned
            if (bounds != null)
            {
                targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
            }

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
