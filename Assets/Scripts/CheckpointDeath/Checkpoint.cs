using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private SpriteRenderer sr;
    private static Checkpoint activeCheckpoint;

    public Color inactiveColor = Color.gray;
    public Color activeColor = Color.green;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = inactiveColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (activeCheckpoint != null)
                activeCheckpoint.Deactivate();

            activeCheckpoint = this;
            Activate();

            PlayerRespawn.SetCheckpoint(transform.position);
        }
    }

    public void Activate()
    {
        sr.color = activeColor;
    }

    public void Deactivate()
    {
        sr.color = inactiveColor;
    }
}
