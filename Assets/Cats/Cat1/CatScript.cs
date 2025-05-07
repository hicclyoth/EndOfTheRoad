using UnityEngine;

public class CatAnimator : MonoBehaviour
{
    private Animator animator;
    private MoveTrap moveTrap;

    void Start()
    {
        animator = GetComponent<Animator>();
        moveTrap = GetComponent<MoveTrap>();
    }

    void Update()
    {
        if (animator != null && moveTrap != null)
        {
            animator.SetBool("IsRunning", moveTrap.IsMoving());
        }
    }
}
