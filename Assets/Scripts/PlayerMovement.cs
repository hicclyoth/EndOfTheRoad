using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpStrength = 16f;    
    [SerializeField] private float jumpCutMultiplier = 0.5f; 
    [SerializeField] private Transform groundCheck;     
    [SerializeField] private LayerMask groundLayer;   

    private Rigidbody2D rb;
    private float moveInput;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovementInput();
        HandleJumpInput();
        FlipCharacter();
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    private void HandleMovementInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal"); 
    }


    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpStrength); 
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier); // Cut jump force
        }
    }

    private void ApplyMovement()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    private void FlipCharacter()
    {
        if (facingRight && moveInput < 0f || !facingRight && moveInput > 0f)
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
