using System;
using UnityEngine;

public class ProximityDetector : MonoBehaviour
{
    public delegate void PlayerStepped();
    public event PlayerStepped OnPlayerStepped;
    [SerializeField] private Transform player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == player)
        {
            OnPlayerStepped?.Invoke();
            Debug.Log("Stepped!");
        }
    }
}