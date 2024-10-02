using UnityEngine;

public class Teehee : MonoBehaviour
{
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void LockerOpening()
    {
        animator.Play("LockerOpening");
    }

    public void LockerHiding()
    {
        animator.Play("LockerHidingIdle");
    }
}
