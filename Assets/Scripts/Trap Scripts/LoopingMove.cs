using UnityEngine;

public class LoopingMover : MonoBehaviour, IResettable
{
    public enum MoveDirection { Horizontal, Vertical }

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDistance = 4f;
    [SerializeField] private MoveDirection moveDirection = MoveDirection.Horizontal;

    private Vector2 startPosition;
    private Vector2 pointA;
    private Vector2 pointB;
    private Vector2 currentTarget;

    private void Start()
    {
        startPosition = transform.position;

        Vector2 direction = moveDirection == MoveDirection.Horizontal ? Vector2.right : Vector2.up;
        pointA = startPosition;
        pointB = startPosition + direction * moveDistance;
        currentTarget = pointB;

        LevelResetManager.Instance.Register(this);
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentTarget, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, currentTarget) < 0.01f)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
        }
    }

    public void ResetState()
    {
        transform.position = startPosition;
        currentTarget = pointB;
    }
}
