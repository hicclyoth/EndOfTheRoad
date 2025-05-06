using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransfer : MonoBehaviour
{
    public string sceneToLoad;
    public float delay = 1.5f;
    public ParticleSystem transferParticles;

    private bool hasTriggered = false;

    public Transform playerTransform;

    public void TriggerTransfer()
    {
        if (hasTriggered) return;
        hasTriggered = true;

        if (CameraShaker.Instance != null)
            CameraShaker.Instance.Shake();

        DoZoomAndOffsetReset();

        if (transferParticles != null)
            transferParticles.Play();

        StartCoroutine(DelayedTransfer());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (CameraShaker.Instance != null)
                CameraShaker.Instance.Shake();

            DoZoomAndOffsetReset();

            Player player = other.GetComponent<Player>();
            if (player != null)
                player.DisablePlayer();

            if (transferParticles != null)
                transferParticles.Play();

            StartCoroutine(DelayedTransfer());
        }
    }
    private void DoZoomAndOffsetReset()
    {
        CameraFollow camFollow = null;
        if (Camera.main != null)
            camFollow = Camera.main.GetComponent<CameraFollow>();
        if (camFollow == null)
            camFollow = Camera.main?.GetComponentInParent<CameraFollow>();
        if (camFollow == null)
            camFollow = GameObject.FindObjectOfType<CameraFollow>();

        if (camFollow == null)
        {
            Debug.LogWarning("CameraFollow script not found anywhere!");
            return;
        }

        camFollow.StopFollowing();
        camFollow.IgnoreBounds(true);
        camFollow.ResetOffset();

        Transform camT = camFollow.transform;

        if (playerTransform != null)
        {
            Vector3 playerPos = playerTransform.position;
            camT.position = new Vector3(playerPos.x, playerPos.y, -10f);
        }
        else
        {
            Debug.LogWarning("PlayerTransform not assigned in LevelTransfer.");
        }

        camFollow.ZoomIn();
    }

    private System.Collections.IEnumerator DelayedTransfer()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
    }
}
