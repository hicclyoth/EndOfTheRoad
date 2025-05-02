using UnityEngine;

public class DisableObject : MonoBehaviour
{
    public float delay = 0f;

    public void Disable()
    {
        Invoke(nameof(DoDisable), delay);
    }

    private void DoDisable()
    {
        gameObject.SetActive(false);
    }
}
