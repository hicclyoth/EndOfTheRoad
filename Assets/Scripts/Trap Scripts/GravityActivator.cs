using UnityEngine;

public class GravityActivator : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float gravityScale = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void ActivateGravity()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravityScale;
    }
}
