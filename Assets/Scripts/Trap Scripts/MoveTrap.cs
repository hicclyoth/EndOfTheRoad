using UnityEngine;

public class MoveTrap : MonoBehaviour, IResettable
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
        // Register this trap with the LRM to allow reset functionality
        startPosition = transform.position;
        LevelResetManager.Instance.Register(this);
    }

    private void Update()
    {
        // If the trap is moving, update its position towards the target position
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Stop moving when the trap reaches the target position
            if ((Vector2)transform.position == targetPosition)
            {
                isMoving = false;
            }
        }
    }

    // Starts the movement of the trap to its target position
    public void StartMoving()
    {
        targetPosition = (Vector2)transform.position + GetDirectionVector() * moveDistance;
        isMoving = true;
    }

    // Get the direction for movement
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

    // When a player collides with the trap, push the player away
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 pushDirection = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }
    }

    // This method will be called when the game is reset
    public void ResetState()
    {
        transform.position = startPosition; // Reset trap position
        isMoving = false; // Stop the trap from moving
    }
}
