using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class VoiceoverSequenceManager : MonoBehaviour
{
    [Header("Voiceover Settings")]
    public AudioClip[] voiceoverClips;  // Array of voiceover clips to play

    [Header("Post Voiceover Actions")]
    public UnityEvent onVoiceoversComplete;  // Event triggered when all voiceovers are complete
    public UnityAction onVoiceoverFinished;  // Action triggered after each voiceover finishes

    private AudioSource audioSource;  // AudioSource component to play the voiceovers

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();  // Get the AudioSource component
    }

    // Start the voiceover sequence
    public void PlayVoiceovers()
    {
        StartCoroutine(PlayVoiceoversSequence());  // Begin playing the voiceovers
    }

    private IEnumerator PlayVoiceoversSequence()
    {
        foreach (AudioClip clip in voiceoverClips)  // Loop through each voiceover clip
        {
            audioSource.clip = clip;  // Set the audio clip
            audioSource.Play();  // Play the audio clip
            yield return new WaitForSeconds(clip.length);  // Wait for the clip to finish

            onVoiceoverFinished?.Invoke();  // Trigger action after each voiceover finishes
        }

        onVoiceoversComplete?.Invoke();  // Trigger event after all voiceovers are complete
    }
}
