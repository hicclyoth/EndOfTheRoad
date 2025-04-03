using UnityEngine;

public class ProximityDetector : MonoBehaviour
{
    public delegate void PlayerNearby();
    public event PlayerNearby OnPlayerDetected;

    [SerializeField] private Transform player;
    [SerializeField] private float activationDistance = 5f;

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < activationDistance)
        {
            OnPlayerDetected?.Invoke(); 
        }
    }
}
