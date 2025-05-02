using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransfer : MonoBehaviour
{
    public string sceneToLoad;
    public float delay = 1.5f;
    public ParticleSystem transferParticles;

    private bool hasTriggered = false;

    // Trigger from UnityEvent (e.g., button)
    public void TriggerTransfer()
    {
        if (hasTriggered) return;
        hasTriggered = true;

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

            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.DisablePlayer();
            }

            if (transferParticles != null)
                transferParticles.Play();

            StartCoroutine(DelayedTransfer());
        }
    }

    private System.Collections.IEnumerator DelayedTransfer()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
    }
}
