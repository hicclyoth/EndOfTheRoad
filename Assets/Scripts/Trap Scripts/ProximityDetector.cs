using UnityEngine;
using UnityEngine.Events;

public class ProximityDetector : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private UnityEvent onPlayerEnter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == player)
        {
            onPlayerEnter.Invoke(); 
        }
    }
}
