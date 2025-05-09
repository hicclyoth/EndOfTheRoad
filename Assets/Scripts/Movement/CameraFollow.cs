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
    private Coroutine zoomCoroutine;

    void Start()
    {
        if (bounds != null)
        {
            Bounds b = bounds.bounds;
            minPosition = b.min;
            maxPosition = b.max;
        }
        cam = GetComponentInChildren<Camera>();
        if (cam == null)
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
                transform.position.z  // Keep the original z position
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
        {
            // Stop any existing zoom coroutine
            if (zoomCoroutine != null)
                StopCoroutine(zoomCoroutine);

            zoomCoroutine = StartCoroutine(ZoomInCoroutine(targetSize, speed));
        }
        else
        {
            Debug.LogWarning("Camera reference is null, cannot zoom!");
        }
    }

    private System.Collections.IEnumerator ZoomInCoroutine(float targetSize, float speed)
    {
        IgnoreBounds(true);
        float startSize = cam.orthographicSize;
        float elapsedTime = 0f;
        float duration = 1.0f; // Duration for smooth zoom, adjust as needed

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime * speed;
            float t = Mathf.Clamp01(elapsedTime / duration);
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            if (Mathf.Abs(cam.orthographicSize - targetSize) < 0.01f)
                break;

            yield return null;
        }

        // Ensure we reach exact target size
        cam.orthographicSize = targetSize;
        zoomCoroutine = null;
    }

    public void IgnoreBounds(bool value) => ignoreBounds = value;

    public void ResetZoom()
    {
        if (cam != null)
        {
            // Stop any existing zoom coroutine
            if (zoomCoroutine != null)
                StopCoroutine(zoomCoroutine);

            cam.orthographicSize = originalSize;
        }
    }
}