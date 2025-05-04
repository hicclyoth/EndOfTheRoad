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

        if (isGrounded && !wasGrounded)
        {
            animator.SetTrigger("Land");
        }
        wasGrounded = isGrounded;
    }

    void HandleMovement()
    {
        float move = 0f;

        if (leftButton != null && leftButton.IsPressed)
            move = -1f;
        else if (rightButton != null && rightButton.IsPressed)
            move = 1f;

        if (move < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (move > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); 
        }

        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
    }

    void HandleJump(bool isGrounded)
    {
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
