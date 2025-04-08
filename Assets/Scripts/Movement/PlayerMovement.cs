using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    public TouchButton leftButton;
    public TouchButton rightButton;
    public TouchButton jumpButton;

    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool jumpUsed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    void HandleMovement()
    {
        float move = 0f;

        if (leftButton != null && leftButton.IsPressed)
            move = -1f;
        else if (rightButton != null && rightButton.IsPressed)
            move = 1f;

        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (jumpButton != null && jumpButton.IsPressed && isGrounded && !jumpUsed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpUsed = true;
        }

        if (!jumpButton.IsPressed && isGrounded)
        {
            jumpUsed = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}
