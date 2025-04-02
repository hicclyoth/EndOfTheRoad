using UnityEngine;

public class MovingSprite : MonoBehaviour
{
    [SerializeField] private Transform player;  // Reference to the player
    [SerializeField] private float activationDistance = 5f; // Distance at which it moves
    [SerializeField] private float moveSpeed = 2f; // Speed of movement

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool isMoving = false;

    private void Start()
    {
        startPosition = transform.position;
        targetPosition = new Vector2(startPosition.x + 4f, startPosition.y); // Moves 4 units to the right
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < activationDistance)
        {
            isMoving = true; // Start moving when player is close
        }

        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
