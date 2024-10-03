using UnityEngine;
using FMODUnity;

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
    
    private void UpdateHideSound()
    {
        RuntimeManager.PlayOneShot("event:/Ambiance and SFX/sfx_hidingNoise");
    }
    private void UpdateSlamSound()
    {
        RuntimeManager.PlayOneShot("event:/Ambiance and SFX/sfx_hidingSlam");
    }
}
