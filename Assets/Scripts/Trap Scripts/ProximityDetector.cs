using UnityEngine;
using UnityEngine.Events;

public class ProximityDetector : MonoBehaviour
{
    private string playerTag = "Player";
    [SerializeField] private UnityEvent onPlayerEnter;

    private Transform player;

    private void Start()
    {
        GameObject foundPlayer = GameObject.FindGameObjectWithTag(playerTag);
        if (foundPlayer != null)
        {
            player = foundPlayer.transform;
        }
        else
        {
            Debug.LogWarning("ProximityDetector: No GameObject with tag '" + playerTag + "' found.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == player)
        {
            onPlayerEnter.Invoke();
        }
    }
}
