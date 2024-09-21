using UnityEngine;

public class FrogButton : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnClick()
    {
        animator.SetTrigger("Click");
    }

    public void ResetState()
    {
        animator.SetTrigger("Reset");
    }
}