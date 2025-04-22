using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject deathParticles;

    public void Die()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
    }
}

