using UnityEngine;

public class FloatyDrift : MonoBehaviour
{
    public float gravity = -0.1f;
    public float maxFallSpeed = -0.3f;
    public float bounceForce = 0.6f;
    public float bounceInterval = 1.5f;
    public float driftSpeed = -0.5f;
    public float rotationSpeed = 30f; // Degrees per second

    private Rigidbody2D rb;
    private float bounceTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    void FixedUpdate()
    {
        Vector2 velocity = rb.linearVelocity;

        // Floaty gravity
        if (velocity.y > maxFallSpeed)
        {
            velocity.y += gravity * Time.fixedDeltaTime;
        }

        // Constant leftward drift
        velocity.x = driftSpeed;
        rb.linearVelocity = velocity;

        // Periodic upward bounce
        bounceTimer += Time.fixedDeltaTime;
        if (bounceTimer >= bounceInterval)
        {
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            bounceTimer = 0f;
        }

        // Rotate the object
        transform.Rotate(0f, 0f, rotationSpeed * Time.fixedDeltaTime);
    }
}
