using UnityEngine;

public class DeleteTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player (or any specific object)
        if (other.CompareTag("Player"))
        {
            // Destroy the trigger immediately
            Destroy(gameObject);
        }
    }
}
