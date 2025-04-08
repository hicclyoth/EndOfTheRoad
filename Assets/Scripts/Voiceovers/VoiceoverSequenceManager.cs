using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class VoiceoverSequenceManager : MonoBehaviour
{
    [Header("Voiceover Settings")]
    public AudioClip[] voiceoverClips;
    public float minVoiceoverDuration = 20f;  

    [Header("Post Voiceover Actions")]
    public UnityEvent onVoiceoversComplete; 
    public UnityAction onVoiceoverFinished;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void PlayVoiceovers()
    {
        StartCoroutine(PlayVoiceoversSequence());
    }

    private IEnumerator PlayVoiceoversSequence()
    {
        foreach (AudioClip clip in voiceoverClips)
        {
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitForSeconds(Mathf.Max(minVoiceoverDuration, clip.length));

            onVoiceoverFinished?.Invoke();
        }

        onVoiceoversComplete?.Invoke();
    }
}
