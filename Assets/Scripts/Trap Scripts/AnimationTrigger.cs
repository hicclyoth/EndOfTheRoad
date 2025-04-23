using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void PlayOnceAnimation()
    {
        animator.SetTrigger("PlayOnce");
    }
}
