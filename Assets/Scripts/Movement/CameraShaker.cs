using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance;

    private Vector3 originalLocalPos;
    private Coroutine shakeCoroutine;
    private bool isShaking = false;

    private void Awake()
    {
        Instance = this;
        originalLocalPos = transform.localPosition;
    }

    public void Shake(float duration = 0.5f, float magnitude = 0.2f)
    {
        StopShake();
        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    public void ShakeUntilStopped(float magnitude = 0.2f)
    {
        StopShake();
        shakeCoroutine = StartCoroutine(ShakeUntilStoppedCoroutine(magnitude));
    }

    public void StopShake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }

        transform.localPosition = originalLocalPos;
        isShaking = false;
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector3 shakeOffset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0f
            ) * magnitude;

            transform.localPosition = originalLocalPos + shakeOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalLocalPos;
        isShaking = false;
    }

    private IEnumerator ShakeUntilStoppedCoroutine(float magnitude)
    {
        isShaking = true;

        while (true)
        {
            Vector3 shakeOffset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0f
            ) * magnitude;

            transform.localPosition = originalLocalPos + shakeOffset;

            yield return null;
        }
    }

    public void UpdateOriginalPosition()
    {
        if (!isShaking)
            originalLocalPos = transform.localPosition;
    }
}
