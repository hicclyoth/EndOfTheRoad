using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 lastCheckpointPosition;

    private void Start()
    {
        lastCheckpointPosition = transform.position;
    }

    public void SetCheckpoint(Vector2 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
    }

    public void RespawnPlayer()
    {
        transform.position = lastCheckpointPosition;
    }

}
