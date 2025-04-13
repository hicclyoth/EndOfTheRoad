using UnityEngine;

//Self explanatory
public class Checkpoint : MonoBehaviour
{    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.SetCheckpoint(transform.position);
            }
        }
    }
}
