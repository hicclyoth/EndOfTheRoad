using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance;
    private Vector3 originalPos;
    private Coroutine shakeCoroutine;
    private bool isShaking = false;

    private void Awake()
    {
        Instance = this;
        originalPos = transform.localPosition;
    }

    public void Shake(float duration = 0.5f, float magnitude = 0.2f)
    {
        Debug.Log("Camera shake triggered: duration=" + duration + ", magnitude=" + magnitude);

        // Stop any existing shake
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            transform.localPosition = originalPos;
        }

        // Store the current position as original if we're not already shaking
        if (!isShaking)
        {
            originalPos = transform.localPosition;
        }

        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private System.Collections.IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Use a gentler damping for longer shakes to maintain shake effect throughout
            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp01(2.0f * percentComplete - 1.5f);

            // Generate noise values for shake
            float x = Random.Range(-1f, 1f) * magnitude * damper;
            float y = Random.Range(-1f, 1f) * magnitude * damper;

            // Apply shake offset to original position
            transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset to original position
        transform.localPosition = originalPos;
        isShaking = false;
        shakeCoroutine = null;
    }

    // Call this if you need to update the original position (e.g., if camera moved)
    public void UpdateOriginalPosition()
    {
        if (!isShaking)
        {
            originalPos = transform.localPosition;
        }
    }
}