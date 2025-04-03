using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 28f;
    [SerializeField] private float moveForce = 4f;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool isMoving = false;

    private void Start()
    {
        startPosition = transform.position;
        targetPosition = new Vector2(startPosition.x + moveForce, startPosition.y); 

        ProximityDetector detector = FindAnyObjectByType<ProximityDetector>();
        if (detector != null)
        {
            detector.OnPlayerStepped += StartMoving;
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void StartMoving()
    {
        isMoving = true;
    }
}
