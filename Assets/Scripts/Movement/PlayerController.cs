using UnityEngine;
using UnityEngine.Audio;

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
    public AudioClip musicClip;
    public AudioMixerGroup sfxGroup;
    public AudioMixerGroup musicGroup;
    public float walkSoundDelay = 0.4f;

    private AudioSource audioSource;
    private AudioSource musicSource;
    private float walkSoundTimer;

    private Rigidbody2D rb;
    private Animator animator;

    private bool jumpUsed = false;
    private bool isDead = false;
    private bool isGrounded;
    private float move = 0f;
    private Vector3 originalScale;
    private bool facingRight = true;

    private float groundCheckTimer;
    private float animationUpdateTimer;

    private int speedHash;
    private int isGroundedHash;
    private int yVelocityHash;
    private int landHash;

    private Vector2 cachedVelocity = Vector2.zero;
    private Vector2 jumpVelocity = Vector2.zero;
    private bool wasGrounded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        originalScale = transform.localScale;

        groundCheckTimer = groundCheckInterval;
        animationUpdateTimer = animationUpdateInterval;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxGroup;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = musicClip;
        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.loop = true;
        musicSource.playOnAwake = false;

        if (animator != null)
        {
            speedHash = Animator.StringToHash("Speed");
            isGroundedHash = Animator.StringToHash("IsGrounded");
            yVelocityHash = Animator.StringToHash("YVelocity");
            landHash = Animator.StringToHash("Land");
        }

        if (!enableAnimations && animator != null)
        {
            animator.enabled = false;
        }
    }

    private void Start()
    {
        if (musicSource != null && musicClip != null)
        {
            musicSource.Play();
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

        bool previouslyGrounded = isGrounded;

        if (groundCheckTimer <= 0)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
            groundCheckTimer = groundCheckInterval;
        }
        else
        {
            groundCheckTimer -= Time.fixedDeltaTime;
        }

        if (!previouslyGrounded && isGrounded && animator != null)
        {
            animator.SetTrigger(landHash);
        }

        ApplyMovement();

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
        move = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || (leftButton != null && leftButton.IsPressed))
            move = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || (rightButton != null && rightButton.IsPressed))
            move = 1f;
    }

    private void ApplyMovement()
    {
        if (usePhysicsMovement)
        {
            if (move != 0)
            {
                rb.AddForce(new Vector2(move * moveSpeed, 0f), ForceMode2D.Force);

                float clampedX = Mathf.Clamp(rb.linearVelocity.x, -maxSpeed, maxSpeed);
                if (rb.linearVelocity.x != clampedX)
                {
                    rb.linearVelocity = new Vector2(clampedX, rb.linearVelocity.y);
                }
            }
            else
            {
                Vector2 vel = rb.linearVelocity;
                vel.x *= 0.9f;
                rb.linearVelocity = vel;
            }
        }
        else
        {
            cachedVelocity.x = move * moveSpeed;
            cachedVelocity.y = rb.linearVelocity.y;
            rb.linearVelocity = cachedVelocity;
        }

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
        if (!isGrounded) return;

        bool kbJump = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool touchJump = jumpButton != null && jumpButton.IsPressed;

        if ((kbJump || touchJump) && !jumpUsed)
        {
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

        animator.SetFloat(speedHash, Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool(isGroundedHash, isGrounded);
        animator.SetFloat(yVelocityHash, rb.linearVelocity.y);
    }

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
