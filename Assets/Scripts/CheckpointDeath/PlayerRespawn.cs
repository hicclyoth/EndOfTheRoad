using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Vector2 lastCheckpointPosition;  // Store the last checkpoint position
    private PlayerController controller;

    [Header("UI")]
    [SerializeField] private GameObject respawnPanel;  // UI panel for respawn

    [Header("Audio")]
    private AudioClip deathClipToPlay;    // Store the death sound clip
    private AudioSource audioSource;      // AudioSource to play the sound

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        lastCheckpointPosition = transform.position;  // Initialize checkpoint to start position

        if (respawnPanel != null)
            respawnPanel.SetActive(false);  // Hide respawn UI on start

        audioSource = GetComponent<AudioSource>();  // Get the AudioSource component attached to the player
    }

    // Update the last checkpoint position when the player reaches a checkpoint
    public void SetCheckpoint(Vector2 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
    }

    // Method to trigger the respawn with the passed audio clip
    public void RespawnWithAudio(AudioClip deathClip)
    {
        deathClipToPlay = deathClip;  // Set the death audio clip
        StartCoroutine(RespawnPlayer());  // Start respawn coroutine
        if (controller != null)
            controller.SetDead(true);
    }

    // Method called from UnityEvent when a trap triggers the player's death
    public void TriggerDeathWithClip(AudioClip clip)
    {
        RespawnWithAudio(clip);  // Call RespawnWithAudio with the specific death clip
    }

    // Start the respawn process
    private IEnumerator RespawnPlayer()
    {
        // Immediately play the death sound when the player dies
        if (deathClipToPlay != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathClipToPlay);  // Play the death sound immediately
        }

        // Freeze the game time (pause the game)
        //Time.timeScale = 0f;

        // Show the respawn UI
        if (respawnPanel != null)
            respawnPanel.SetActive(true);

        // Wait for the player to release input
        while (Input.GetMouseButton(0) || Input.touchCount > 0)
            yield return null;

        bool tapped = false;
        // Wait for a fresh tap to resume
        while (!tapped)
        {
            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
                tapped = true;

            yield return null;
        }

        //Time.timeScale = 1f;  // Unfreeze the time (resume the game)
        transform.position = lastCheckpointPosition;  // Move the player to the last checkpoint

        if (respawnPanel != null)
            respawnPanel.SetActive(false);  // Hide the respawn UI

        // Reset the level (traps, platforms, etc.)
        LevelResetManager.Instance.ResetLevel();
        if (controller != null)
            controller.SetDead(false);

    }
}
