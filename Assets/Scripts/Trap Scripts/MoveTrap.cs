using UnityEngine;

public class MoveTrap : MonoBehaviour
{
    public enum MoveDirection { Left, Right, Up, Down } // Dropdown for direction

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private MoveDirection direction; // Dropdown for movement direction
    [SerializeField] private float moveDistance = 4f; // How far it moves

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool isMoving = false;

    private void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + GetDirectionVector() * moveDistance;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }

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
}
