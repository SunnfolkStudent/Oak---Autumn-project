using UnityEngine;

public class SpaceShuttle_AnimatorController : MonoBehaviour
{
    public PlayerController playerController;
    public Animator animator;
    public MenuScript menuScript;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayerEscaped()
    {
        animator.Play("SpaceShuttleEscaped");
    }

    public void ToVictoryScreen()
    {
        menuScript.VictoryScreen();
    }
}