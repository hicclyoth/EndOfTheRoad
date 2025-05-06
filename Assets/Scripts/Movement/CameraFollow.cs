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

    private Camera cam;
    private float originalSize;
    private bool ignoreBounds = false;

    void Start()
    {
        if (bounds != null)
        {
            Bounds b = bounds.bounds;
            minPosition = b.min;
            maxPosition = b.max;
        }

        cam = Camera.main;
        if (cam != null)
            originalSize = cam.orthographicSize;
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

            if (!ignoreBounds && bounds != null)
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

    public void ResetOffset() => offset = Vector2.zero;

    public void ZoomIn(float targetSize = 2.5f, float speed = 3f)
    {
        if (cam != null)
            StartCoroutine(ZoomInCoroutine(targetSize, speed));
    }

    private System.Collections.IEnumerator ZoomInCoroutine(float targetSize, float speed)
    {
        IgnoreBounds(true);
        while (cam.orthographicSize > targetSize)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void IgnoreBounds(bool value) => ignoreBounds = value;
}
