using UnityEngine;

public class DisableObject : MonoBehaviour, IResettable
{
    public float delay = 0f;

    private bool isDisabled = false;

    private void Start()
    {
        LevelResetManager.Instance.Register(this);
    }

    public void Disable()
    {
        Invoke(nameof(DoDisable), delay);
    }

    private void DoDisable()
    {
        gameObject.SetActive(false);
        isDisabled = true;
    }

    public void ResetState()
    {
        if (isDisabled)
        {
            gameObject.SetActive(true);
            isDisabled = false;
        }
    }
}
