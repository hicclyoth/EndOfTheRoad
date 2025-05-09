using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransfer : MonoBehaviour
{
    public string sceneToLoad;
    public float delay = 1.5f;
    public ParticleSystem transferParticles;
    public Transform playerTransform;
    public float zoomTargetSize = 2.5f;
    public float zoomSpeed = 3f;

    private bool hasTriggered = false;

    public void TriggerTransfer()
    {
        if (hasTriggered) return;
        hasTriggered = true;

        // Shake camera for the entire duration of the transfer delay
        if (CameraShaker.Instance != null)
            CameraShaker.Instance.Shake(duration: delay, magnitude: 0.2f);

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

            if (playerTransform == null)
                playerTransform = other.transform;

            // Shake camera for the entire duration of the transfer delay
            if (CameraShaker.Instance != null)
                CameraShaker.Instance.Shake(duration: delay, magnitude: 0.2f);

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
        // Find the camera follow script
        CameraFollow camFollow = FindCameraFollow();

        if (camFollow == null)
        {
            Debug.LogWarning("CameraFollow script not found anywhere!");
            return;
        }

        // Stop camera from following player
        camFollow.StopFollowing();
        camFollow.IgnoreBounds(true);
        camFollow.ResetOffset();

        // When player contains the camera as a child, we need to handle positioning differently
        if (playerTransform != null)
        {
            // Find the actual player sprite position (might be a child of playerTransform)
            Transform spriteTransform = FindPlayerSpriteTransform(playerTransform);
            Vector3 targetPosition = spriteTransform != null ? spriteTransform.position : playerTransform.position;

            // Keep the camera's original z value
            float camZ = camFollow.transform.position.z;

            // Set camera position to player's position
            camFollow.transform.position = new Vector3(targetPosition.x, targetPosition.y, camZ);

            Debug.Log("Camera positioned at: " + camFollow.transform.position + " based on player at: " + targetPosition);
        }
        else
        {
            Debug.LogWarning("PlayerTransform not assigned in LevelTransfer.");
        }

        // Trigger zoom effect
        camFollow.ZoomIn(zoomTargetSize, zoomSpeed);
    }

    // Try to find the player's sprite transform (often a child of player)
    private Transform FindPlayerSpriteTransform(Transform playerTransform)
    {
        // First check if there's a SpriteRenderer component on a child
        SpriteRenderer spriteRenderer = playerTransform.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.transform != playerTransform)
        {
            return spriteRenderer.transform;
        }

        // If not found, look for any child named "Sprite" or containing "Sprite"
        foreach (Transform child in playerTransform)
        {
            if (child.name.Contains("Sprite"))
            {
                return child;
            }
        }

        // If no sprite-specific transform found, return the player transform itself
        return playerTransform;
    }

    private CameraFollow FindCameraFollow()
    {
        // Try to find in main camera first
        CameraFollow camFollow = null;

        if (Camera.main != null)
        {
            // Try to get component directly on the camera
            camFollow = Camera.main.GetComponent<CameraFollow>();

            // If not found, try parent
            if (camFollow == null && Camera.main.transform.parent != null)
                camFollow = Camera.main.transform.parent.GetComponent<CameraFollow>();
        }

        // If still not found, search entire scene
        if (camFollow == null)
            camFollow = GameObject.FindObjectOfType<CameraFollow>();

        return camFollow;
    }

    private System.Collections.IEnumerator DelayedTransfer()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
    }
}