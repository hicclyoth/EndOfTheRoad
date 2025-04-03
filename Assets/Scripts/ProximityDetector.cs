using UnityEngine;

public class ProximityDetector : MonoBehaviour
{
    [SerializeField] private Transform player; // Assign the player manually
    [SerializeField] private MoveTrap targetTrap; // Assign the specific trap to trigger

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == player) // Only detect the assigned player
        {
            targetTrap.StartMoving(); // Directly call StartMoving() on the assigned trap
        }
    }
}