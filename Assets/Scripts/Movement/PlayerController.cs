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

    private bool isDead = false;

    private Animator animator;
    private bool wasGrounded = true;
    private bool isLanding = false;
    private bool justJumped = false;
    private bool landingLocked = false;


    public void SetDead(bool dead)
    {
        isDead = dead;
        rb.linearVelocity = Vector2.zero; // Stop movement immediately
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Debug.Log("Animator found: " + animator);

    }

    void Update()
    {
        if (isDead) return; // Block input if dead

        HandleMovement();
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        HandleMovement();
        HandleJump(isGrounded);

        // Update Animator Parameters
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("YVelocity", rb.linearVelocity.y);

        bool isFalling = rb.linearVelocity.y < -0.1f;

        if (isGrounded && !wasGrounded)
        {
            animator.SetTrigger("Land");
        }



        wasGrounded = isGrounded;
    }

    void UnlockLanding()
    {
        landingLocked = false;
    }

    public void OnLandAnimationComplete()
    {
        isLanding = false;
    }

    void HandleMovement()
    {
        float move = 0f;

        if (leftButton != null && leftButton.IsPressed)
            move = -1f;
        else if (rightButton != null && rightButton.IsPressed)
            move = 1f;

        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        // Flip only X scale without touching Y/Z
        if (move != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = move < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }


    void HandleJump(bool isGrounded)
    {
        if (jumpButton != null && jumpButton.IsPressed && isGrounded && !jumpUsed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpUsed = true;
            justJumped = true;
            Invoke(nameof(ResetJustJumped), 0.1f); // buffer to block accidental landing
        }

        if (!jumpButton.IsPressed && isGrounded)
        {
            jumpUsed = false;
        }
    }

    void ResetJustJumped()
    {
        justJumped = false;
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
