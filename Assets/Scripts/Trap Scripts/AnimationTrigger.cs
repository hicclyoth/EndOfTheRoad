using UnityEngine;

public class AnimationTrigger : MonoBehaviour, IResettable
{
    private Animator animator;

    [SerializeField] private string idleStateName = "Idle";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (LevelResetManager.Instance != null)
        {
            LevelResetManager.Instance.Register(this);
        }
    }

    public void PlayOnceAnimation()
    {
        animator.SetTrigger("PlayOnce");
    }

    public void ResetState()
    {
        animator.Play(idleStateName, 0, 0f);
    }
}
