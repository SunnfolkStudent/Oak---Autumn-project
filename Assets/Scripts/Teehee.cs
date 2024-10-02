using UnityEngine;

public class Teehee : MonoBehaviour
{
    public PlayerController playerController;
    public Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayerHides()
    {
        animator.Play("PlayerHides");
    }

    public void PlayerUnhides()
    {
        animator.Play("PlayerUnhides");
    }
}
