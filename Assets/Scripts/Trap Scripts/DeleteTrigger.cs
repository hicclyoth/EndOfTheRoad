using UnityEngine;

public class DeleteTrigger : MonoBehaviour, IResettable
{
    private void Start()
    {
        LevelResetManager.Instance.Register(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false); 
        }
    }

    public void ResetState()
    {
        gameObject.SetActive(true); 
    }
}
