using UnityEngine;

public class MoveTrap : MonoBehaviour
{
    public enum MoveDirection { Left, Right, Up, Down }

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private MoveDirection direction;
    [SerializeField] private float moveDistance = 4f;
    [SerializeField] private float pushForce = 5f;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool isMoving = false;

    private void Start()
    {
        startPosition = transform.position; // Set initial position at the start
    }

    private void Update()
    {
        // If the trap is moving, update its position
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Stop moving once the target position is reached
            if ((Vector2)transform.position == targetPosition)
            {
                isMoving = false;
            }
        }
    }

    // This method will be called when the trap should start moving
    public void StartMoving()
    {
        // Calculate the target position based on the current position and movement direction
        targetPosition = (Vector2)transform.position + GetDirectionVector() * moveDistance;

        isMoving = true;
    }

    // This method gives the direction vector based on the movement direction
    private Vector2 GetDirectionVector()
    {
        switch (direction)
        {
            case MoveDirection.Left: return Vector2.left;
            case MoveDirection.Right: return Vector2.right;
            case MoveDirection.Up: return Vector2.up;
            case MoveDirection.Down: return Vector2.down;
            default: return Vector2.zero;
        }
    }

    // This method is called when the trap collides with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Push the player away from the trap
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}
