using UnityEngine;
using UnityEngine.Events;

public class ProximityDetector : MonoBehaviour
{
    [SerializeField] private Transform player; // The player GameObject
    [SerializeField] private UnityEvent onPlayerEnter; // Unity Event to trigger custom actions

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == player)
        {
            onPlayerEnter.Invoke(); // Trigger the assigned UnityEvent
        }
    }
}
