using UnityEngine;

public class MoveTrap : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveForce = 4f;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool isMoving = false;

    private void Start()
    {
        startPosition = transform.position;
        targetPosition = new Vector2(startPosition.x + moveForce, startPosition.y); 

        ProximityDetector detector = GetComponent<ProximityDetector>();
        if (detector != null)
        {
            detector.OnPlayerDetected += StartMoving;
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
