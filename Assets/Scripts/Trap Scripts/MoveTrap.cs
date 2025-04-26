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
    private Rigidbody2D rb;
    private bool isMoving = false;

    private Vector2 velocity = Vector2.zero; 

    private void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        LevelResetManager.Instance.Register(this);
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.SmoothDamp(
                transform.position,
                targetPosition,
                ref velocity,
                0.2f,           
                moveSpeed    
            );

            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
                velocity = Vector2.zero;
            }
        }
    }

    public void StartMoving()
    {
        targetPosition = (Vector2)transform.position + GetDirectionVector() * moveDistance;
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

    public void ResetState()
    {
        transform.position = startPosition;
        isMoving = false;
        velocity = Vector2.zero;
        //Physics2D.SyncTransforms();
    }
}
