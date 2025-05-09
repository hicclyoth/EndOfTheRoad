using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float maxSpeed = 5f;
    public float jumpForce = 10f;
    public bool usePhysicsMovement = false;

    [Header("Controls")]
    public TouchButton leftButton;
    public TouchButton rightButton;
    public TouchButton jumpButton;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    private bool landingLocked = false;
    private bool isLanding = false;


    // Performance settings
    [Header("Performance")]
    [Tooltip("How often to check for ground (in seconds)")]
    public float groundCheckInterval = 0.1f;
    [Tooltip("How often to update animations (in seconds)")]
    public float animationUpdateInterval = 0.05f;
    [Tooltip("Enable animation updates")]
    public bool enableAnimations = true;

    [Header("Audio")]
    public AudioClip jumpClip;
    public AudioClip walkClip;
    public UnityEngine.Audio.AudioMixerGroup sfxGroup;
    public float walkSoundDelay = 0.4f;

    private AudioSource audioSource;
    private float walkSoundTimer;


    // Cached components
    private Rigidbody2D rb;
    private Animator animator;

    // State variables
    private bool jumpUsed = false;
    private bool isDead = false;
    private bool isGrounded;
    private float move = 0f;
    private Vector3 originalScale;
    private bool facingRight = true;

    // Optimization variables
    private float groundCheckTimer;
    private float animationUpdateTimer;

    // Animation hashes
    private int speedHash;
    private int isGroundedHash;
    private int yVelocityHash;
    private int landHash;


    // Movement caching
    private Vector2 cachedVelocity = Vector2.zero;
    private Vector2 jumpVelocity = Vector2.zero;
    private bool wasGrounded = false;


    private void Awake()
    {
        // Get components once
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Cache original scale
        originalScale = transform.localScale;

        // Initialize timers
        groundCheckTimer = groundCheckInterval;
        animationUpdateTimer = animationUpdateInterval;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = sfxGroup;

        // Cache animation hashes
        if (animator != null)
        {
            speedHash = Animator.StringToHash("Speed");
            isGroundedHash = Animator.StringToHash("IsGrounded");
            yVelocityHash = Animator.StringToHash("YVelocity");
            landHash = Animator.StringToHash("Land");
        }

        // Disable animator if not needed
        if (!enableAnimations && animator != null)
        {
            animator.enabled = false;
        }
    }

    public void SetDead(bool dead)
    {
        isDead = dead;
        if (dead && rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Update()
    {
        if (isDead) return;

        // Handle input in Update for responsiveness
        GetMovementInput();
        CheckJumpInput();

        if (isGrounded && Mathf.Abs(rb.linearVelocity.x) > 0.3f)
        {
            walkSoundTimer += Time.deltaTime;
            if (walkSoundTimer >= walkSoundDelay)
            {
                audioSource.PlayOneShot(walkClip);
                walkSoundTimer = 0f;
            }
        }
        else
        {
            walkSoundTimer = walkSoundDelay;
        }

    }

    private void FixedUpdate()
    {
        if (isDead) return;

        // Before updating isGrounded
        bool previouslyGrounded = isGrounded;

        // Update isGrounded
        if (groundCheckTimer <= 0)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
            groundCheckTimer = groundCheckInterval;
        }
        else
        {
            groundCheckTimer -= Time.fixedDeltaTime;
        }

        // Detect landing
        if (!previouslyGrounded && isGrounded && animator != null)
        {
            animator.SetTrigger(landHash);
        }


        // Always process movement
        ApplyMovement();

        // Throttle animation updates
        if (enableAnimations && animator != null && animator.enabled)
        {
            if (animationUpdateTimer <= 0)
            {
                UpdateAnimations();
                animationUpdateTimer = animationUpdateInterval;
            }
            else
            {
                animationUpdateTimer -= Time.fixedDeltaTime;
            }
        }
    }

    private void GetMovementInput()
    {
        // Reset movement
        move = 0f;

        // Check for movement input
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || (leftButton != null && leftButton.IsPressed))
            move = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || (rightButton != null && rightButton.IsPressed))
            move = 1f;
    }

    private void ApplyMovement()
    {
        // Apply movement based on method
        if (usePhysicsMovement)
        {
            // Only apply force when there's input
            if (move != 0)
            {
                rb.AddForce(new Vector2(move * moveSpeed, 0f), ForceMode2D.Force);

                // Clamp velocity to maximum speed
                float clampedX = Mathf.Clamp(rb.linearVelocity.x, -maxSpeed, maxSpeed);
                if (rb.linearVelocity.x != clampedX)
                {
                    rb.linearVelocity = new Vector2(clampedX, rb.linearVelocity.y);
                }
            }
            else
            {
                // Apply friction when no input
                Vector2 vel = rb.linearVelocity;
                vel.x *= 0.9f;
                rb.linearVelocity = vel;
            }
        }
        else
        {
            // Direct movement - more efficient
            cachedVelocity.x = move * moveSpeed;
            cachedVelocity.y = rb.linearVelocity.y;
            rb.linearVelocity = cachedVelocity;
        }

        // Only flip when direction changes
        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? originalScale.x : -originalScale.x;
        transform.localScale = scale;
    }

    private void CheckJumpInput()
    {
        if (!isGrounded) return; // Early exit if not grounded

        bool kbJump = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool touchJump = jumpButton != null && jumpButton.IsPressed;

        if ((kbJump || touchJump) && !jumpUsed)
        {
            // Reuse jump velocity object
            jumpVelocity.x = rb.linearVelocity.x;
            jumpVelocity.y = jumpForce;
            rb.linearVelocity = jumpVelocity;
            audioSource.PlayOneShot(jumpClip);
            jumpUsed = true;
        }

        if (!kbJump && !touchJump)
        {
            jumpUsed = false;
        }
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        // Set basic animation parameters
        animator.SetFloat(speedHash, Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool(isGroundedHash, isGrounded);
        animator.SetFloat(yVelocityHash, rb.linearVelocity.y);
    }

    // Performance optimization - disable unnecessary behavior when off-screen
    private void OnBecameInvisible()
    {
        if (animator != null && enableAnimations)
        {
            animator.enabled = false;
        }
    }

    private void OnBecameVisible()
    {
        if (animator != null && enableAnimations)
        {
            animator.enabled = true;
        }
    }

    public void OnLandAnimationComplete() => isLanding = false;

}