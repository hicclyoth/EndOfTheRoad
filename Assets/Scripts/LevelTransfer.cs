using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransfer : MonoBehaviour
{
    public string sceneToLoad;
    public float delay = 1.5f;
    public ParticleSystem transferParticles;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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
