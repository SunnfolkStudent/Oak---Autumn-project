using UnityEngine;

public class SpaceShuttle_AnimatorController : MonoBehaviour
{
    public PlayerController playerController;
    public Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayerEscaped()
    {
        animator.Play("SpaceShuttleEscaped");
    }
}