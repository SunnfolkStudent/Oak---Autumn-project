using UnityEngine;

public class EnemyAI_Test : MonoBehaviour
{
    private enum State
    {
        Patrol,
        Chase
    }
    
    private State state;
    
    
    private Vector2 startingPosition;

    private Rigidbody2D rb;

    private void Awake()
    {
        state = State.Patrol;
    }
    
    void Start()
    {
        startingPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }
    
    

    void Update()
    {
        
    }
}
