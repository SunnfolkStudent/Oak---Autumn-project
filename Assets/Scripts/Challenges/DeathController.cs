using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathController : MonoBehaviour
{

    private Animator animator;
    private GameObject t;

    private 
    void Start()
    {
        t = GameObject.Find("Enemy");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death(int dir)
    {
        if (dir == 1)
            animator.Play("Enemy_KillFromBehind");
        if (dir == 2)
            animator.Play("Enemy_KillFromInFront");
        if (dir == 3)
            animator.Play("Enemy_KillFromRight");
        if (dir == 4)
            animator.Play("Enemy_KillFromLeft");
        t.GetComponent<StateEnemyAI>().enabled = false;

    }

    public void DoTheRestart()
    {
        SceneManager.LoadScene("GameOver");
    }
}
