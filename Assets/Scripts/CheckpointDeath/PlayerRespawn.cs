using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Vector2 lastCheckpointPosition;

    [Header("UI")]
    [SerializeField] private GameObject respawnPanel;

    private void Start()
    {
        lastCheckpointPosition = transform.position;

        // Make sure panel is hidden on start
        if (respawnPanel != null)
        {
            respawnPanel.SetActive(false);
        }
    }

    public void SetCheckpoint(Vector2 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
    }

    public void RespawnPlayer()
    {
        Time.timeScale = 0f;
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        // Freeze time and stop input

        // Show respawn UI
        if (respawnPanel != null)
            respawnPanel.SetActive(true);

        // Wait until the player releases any input (holding down buttons won't trigger a restart)
        while (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            yield return null;
        }

        // Wait for a fresh tap or click
        bool tapped = false;
        while (!tapped)
        {
            // Check for new input after releasing previous input
            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
            {
                tapped = true;
            }
            yield return null;
        }

        // Unfreeze time
        Time.timeScale = 1f;

        // Move the player to the last checkpoint
        transform.position = lastCheckpointPosition;

        // Hide the respawn panel
        if (respawnPanel != null)
            respawnPanel.SetActive(false);

        // Reset the level after respawn
        LevelResetManager.Instance.ResetLevel();
    }
}
