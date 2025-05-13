using UnityEngine;

public class MoveTrap : MonoBehaviour, IResettable
{
    public enum MoveDirection
    {
        Left, Right, Up, Down,
        UpLeft, UpRight, DownLeft, DownRight
    }

    [Header("Movement Settings")]
    [SerializeField] private bool usePhysics = false;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private MoveDirection direction;
    [SerializeField] private float moveDistance = 4f;
    [SerializeField] private float moveDelay = 0f;

    [Header("Push Player Settings")]
    [SerializeField] private float pushForce = 5f;

    [Header("Camera Shake (while moving)")]
    [SerializeField] private bool shakeCameraOnMove = false;
    [SerializeField] private float shakeMagnitude = 0.2f;

    [Header("Rotation")]
    [SerializeField] private bool rotateWhileMoving = false;
    [SerializeField] private float rotationSpeed = 180f;

    [Header("Audio")]
    [SerializeField] private AudioClip startMoveSound;

    private AudioSource audioSource;

    private Vector2 startPosition;
    private Vector2 initialStartPosition;
    private Quaternion initialRotation;

    private Vector2 targetPosition;
    private Rigidbody2D rb;
    private bool isMoving = false;
    private bool hasStartedMoving = false;
    private Vector2 velocity = Vector2.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        initialStartPosition = startPosition;
        initialRotation = transform.rotation;

        LevelResetManager.Instance.Register(this);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (!usePhysics && isMoving)
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
                StopMovement();
            }
        }

        if (rotateWhileMoving && hasStartedMoving)
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (usePhysics && isMoving)
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
            {
                rb.position = targetPosition;
                StopMovement();
            }
        }

        if (usePhysics && rotateWhileMoving && hasStartedMoving)
        {
            rb.MoveRotation(rb.rotation + rotationSpeed * Time.fixedDeltaTime);
        }
    }

    public void StartMoving()
    {
        StartCoroutine(StartMoveAfterDelay());
    }

    private System.Collections.IEnumerator StartMoveAfterDelay()
    {
        yield return new WaitForSeconds(moveDelay);

        targetPosition = startPosition + GetDirectionVector() * moveDistance;
        isMoving = true;
        hasStartedMoving = true;

        if (startMoveSound != null)
            audioSource.PlayOneShot(startMoveSound);

        if (shakeCameraOnMove && CameraShaker.Instance != null)
        {
            CameraShaker.Instance.ShakeUntilStopped(shakeMagnitude);
        }
    }

    private void StopMovement()
    {
        isMoving = false;
        hasStartedMoving = false;
        velocity = Vector2.zero;

        if (shakeCameraOnMove && CameraShaker.Instance != null)
        {
            CameraShaker.Instance.StopShake();
        }
    }

    private Vector2 GetDirectionVector()
    {
        switch (direction)
        {
            case MoveDirection.Left: return Vector2.left;
            case MoveDirection.Right: return Vector2.right;
            case MoveDirection.Up: return Vector2.up;
            case MoveDirection.Down: return Vector2.down;
            case MoveDirection.UpLeft: return (Vector2.up + Vector2.left).normalized;
            case MoveDirection.UpRight: return (Vector2.up + Vector2.right).normalized;
            case MoveDirection.DownLeft: return (Vector2.down + Vector2.left).normalized;
            case MoveDirection.DownRight: return (Vector2.down + Vector2.right).normalized;
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

    public bool IsMoving() => isMoving;

    public void ResetState()
    {
        startPosition = initialStartPosition;

        if (usePhysics)
        {
            rb.position = startPosition;
            rb.rotation = initialRotation.eulerAngles.z;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        else
        {
            transform.position = startPosition;
            transform.rotation = initialRotation;
        }

        isMoving = false;
        hasStartedMoving = false;
        velocity = Vector2.zero;

        if (shakeCameraOnMove && CameraShaker.Instance != null)
        {
            CameraShaker.Instance.StopShake();
        }
    }
}
