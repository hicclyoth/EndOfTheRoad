using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject deathParticles;

    public void Die()
    {
        GameObject fx = Instantiate(deathParticles, transform.position, Quaternion.identity);

        ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            Destroy(fx, ps.main.duration);
        }
    }
}
