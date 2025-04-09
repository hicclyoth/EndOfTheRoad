using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private static Vector3 checkpointPosition;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        checkpointPosition = transform.position; // default spawn
    }

    public static void SetCheckpoint(Vector3 newPos)
    {
        checkpointPosition = newPos;
    }

    public void Respawn()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = checkpointPosition;
    }
}
