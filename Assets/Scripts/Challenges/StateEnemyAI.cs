using System.Collections;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.SceneManagement;

public class StateEnemyAI : MonoBehaviour
{
    // Creating enum with both enemy states; Patrol and Chase'
    private enum State
    {
        Patrol, 
        Chase,
        Dead
    }

    private enum EnemyDirection
    {
        Right,
        Left,
        Up,
        Down
    }
    
    // Public usual variables
    public Transform target;
    public int currentPoint;
    public Transform[] patrolPoints;
    
    
    // Variable that enables the Chase switchcase to function only if true
    public bool canChase;
    
    // Player's hiding variable
    private bool isThePlayerHiding;
    
    // Speed for Chase switchcase
    private float speed = 400f;

    // CoRoutine
    private bool _LFP = false;
    private int coRoutineCounter = 0;
    
    // Speed for Patrol switchcase
    public float patrolSpeed;
    
    // Animation stuffs
    private Animator animator;
    
    // Boolean used to determine what way the enemy is patrolling through the array
    bool isMovingForwards = true;
    public float nextWaypointDistance = 3f;
    
    //Raycast Testing
    private float maxRange = 5f;
    
    // Using State functionality, see the enum above
    
    [SerializeField]
    private State state;
    
    [SerializeField]
    private EnemyDirection enemyDirection;

    // Using A*'s built-in path mechanic
    private Path path;
    
    // defines current waypoint in the path towards the objective
    public int currentWaypoint = 0;
    
    // Defines if end of path is reached, usually calls for 
    bool reachedEndOfPath = false;

    
    // Another A* built in function
    Seeker seeker;
    
    // Just the enemy RigidBody and its CircleCollider
    private Rigidbody2D rb;
    private CircleCollider2D cc;
    private GameObject o;
    private GameObject t;
    private GameObject e;
    private Animator enemySprite;
    private PlayerController playerController;
    private Animator enemyAnimator;
    private Rigidbody2D playerrb2d;
    


    void Start()
    {
        // Getting PlayerController Script
        o = GameObject.Find("Player");
        t = GameObject.Find("/Enemy/Monster");
        e = GameObject.Find("Enemy");
        playerController = o.GetComponent<PlayerController>();
        enemySprite = t.GetComponent<Animator>();
        
        

        // Getting components
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cc = GetComponent<CircleCollider2D>();
        playerrb2d = o.GetComponent<Rigidbody2D>();
        
        LayerMask mask = LayerMask.GetMask("Player");
        
        // Setting destination to first patrolpoint in array (since currentPoint is set to 0 by default)
        Vector3 patrolDestination = patrolPoints[currentPoint].position;
        currentPoint = 0;
        
        // Using enum state to make Patrol the default state
        state = State.Patrol;
        
        // Repeats mentioned void function with 0f cooldown and .5f seconds between each repetition
        InvokeRepeating("UpdatePatrolPoint", 0f, .5f);
        InvokeRepeating("UpdateSprite", 0f, .1f);
    }
    
    

    void UpdatePatrolPoint()
    {

        switch (state)
        {
            case State.Dead:
                break;
                // Switch state containing the Patrol logic
                case State.Patrol:
                    Vector3 patrolDestination = patrolPoints[currentPoint].position;
        
                    float distanceToPatrolDestination = Vector3.Distance(transform.position, patrolDestination);
                    if (distanceToPatrolDestination < 0.2f)
                    {
                        if (isMovingForwards)
                        {
                            currentPoint++;
                        }
                        else
                        {
                            currentPoint--;
                        }

                        if (currentPoint >= patrolPoints.Length)
                        {
                            isMovingForwards = false;
                            currentPoint = patrolPoints.Length - 1;
                        }

                        if (currentPoint < 0)
                        {
                            isMovingForwards = true;
                            currentPoint = 1;
                        }
                    }
                    if (seeker.IsDone())
                    {
                        seeker.StartPath(transform.position, patrolDestination, OnPathComplete);
                    }
                    break;
            // Switch state containing the Chase logic
            case State.Chase:
                if (canChase)
                {
                    if (seeker.IsDone())
                    {
                        seeker.StartPath(rb.position, target.position, OnPathComplete);
                    }
                }
                break;
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Update()
    {
        switch (state)
        {
            case State.Dead:
                break;
            case State.Patrol:
                cc.radius = 0.6f;
                break;
            case State.Chase:
                cc.radius = 1.2f;
                break;
        }
        
        isThePlayerHiding = playerController.playerIsHiding;
        
        if (isThePlayerHiding)
            canChase = false;

        
        
            
        
    }

    void FixedUpdate()
    {
        
        var horizontalVelocity = rb.linearVelocityX;
        var verticalVelocity = rb.linearVelocityY;
        switch (state)
        {
                case State.Dead:
                break;
                // Part of the code that controls patrolling, refreshes 50 times a second
                case State.Patrol:
                    if (path == null)
                        return;
                    if (currentWaypoint >= path.vectorPath.Count)
                    {
                        reachedEndOfPath = true;
                        return;
                    }
                    else
                    {
                        reachedEndOfPath = false;
                    }
        
                    Vector2 patrolDirection = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                    Vector2 patrolForce = patrolDirection * (patrolSpeed * Time.deltaTime);
        
                    rb.AddForce(patrolForce);
        
                    float patrolDistance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                    if (patrolDistance < nextWaypointDistance)
                    {
                        currentWaypoint++;
                    }

                    break;
            
            // Part of the code that controls chasing the player, refreshes 50 times a second
            case State.Chase: 
                if (path == null)
                    return;
                if (currentWaypoint >= path.vectorPath.Count)
                {
                    if (!_LFP)
                    {
                        StartCoroutine(nameof(Wait1SecLOL));
                        reachedEndOfPath = true;
                        return;
                    }
                    
                    if (_LFP)
                    {
                        reachedEndOfPath = true;
                        return;
                    }
                }
                else
                {
                    reachedEndOfPath = false;
                }
        
                Vector2 chaseDirection = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                Vector2 chaseForce = chaseDirection * (speed * Time.deltaTime);
                rb.AddForce(chaseForce);
        
                float chaseDistance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                if (chaseDistance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
                break;
        }
        
        switch (state)
        {
            case State.Dead:
                break;
            case State.Chase:
                break;
            case State.Patrol:
                if (verticalVelocity > 1f && (horizontalVelocity < 1.2f && horizontalVelocity > -1.2f))
                {
                    enemyDirection = EnemyDirection.Up;
                    break;
                }

                if (verticalVelocity < -1f)
                {
                    enemyDirection = EnemyDirection.Down;
                    break;
                }

                if (horizontalVelocity > 0.01f)
                {
                    enemyDirection = EnemyDirection.Right;
                    break;
                }

                if (horizontalVelocity < -0.01f)
                {
                    enemyDirection = EnemyDirection.Left;
                    break;
                }

                break;
        }
    }

    // WIP WIP WIP WIP WIP 
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && state == State.Chase) 
        {
            
            switch (enemyDirection)
            {
                case EnemyDirection.Down:
                    enemySprite.Play("Enemy_KillFromBehind");
                    
                    
                    break;
                case EnemyDirection.Up:
                    enemySprite.Play("Enemy_KillFromInFront");
                    
                    break;
                case EnemyDirection.Left:
                    enemySprite.Play("Enemy_KillFromRight");
                    
                    break;
                case EnemyDirection.Right:
                    enemySprite.Play("Enemy_KillFromLeft");
                    
                    break;
            }
            print("State is now dead");
            state = State.Dead;
            cc.enabled = false;
            Destroy(other.gameObject);
            




        }
    }
    

    
    // Handles the raycasting when inside of enemy hearing radius
    private void OnTriggerStay2D(Collider2D other)
    {
        switch (state)
        {
            case State.Dead:
                return;
        }
        
        Vector2 targetDir = target.position - transform.position;
        Vector2 enemyPosition = transform.position;
        
        var playerHorizontal = playerrb2d.linearVelocityX;
        var playerVertical = playerrb2d.linearVelocityY;
        
        
        
        
        RaycastHit2D hit = Physics2D.Raycast(enemyPosition, targetDir);

        if (hit.collider.CompareTag("Player") && (playerHorizontal > 0f || playerVertical > 0f))
        {
            print("Found ya!!");
            state = State.Chase;
            canChase = true;
        }
    }
    
    IEnumerator Wait1SecLOL()
    {
        _LFP = true;
        coRoutineCounter += 1;

        if (coRoutineCounter == 5)
            state = State.Patrol;
        
        yield return new WaitForSeconds(1);
        _LFP = false;
    }

    private void UpdateSprite()
    {
        if (state != State.Dead)
        switch (enemyDirection)
        {
            case EnemyDirection.Down:
                enemySprite.Play("Enemy_WalkDown");
                break;
            case EnemyDirection.Up:
                enemySprite.Play("Enemy_WalkUp");
                break;
            case EnemyDirection.Left:
                enemySprite.Play("Enemy_WalkLeft");
                break;
            case EnemyDirection.Right:
                enemySprite.Play("Enemy_WalkRight");
                break;
        }
        
        
        switch (state)
        {
            case State.Dead:
                break;
            case State.Patrol:
                break;
            case State.Chase:
                if (target.position.x - transform.position.x > 2f)
                {
                    enemyDirection = EnemyDirection.Right;
                    print("Left!");
                    break;
                }
                
                if (target.position.x + 2f <= transform.position.x)
                {
                    enemyDirection = EnemyDirection.Left;
                    print("Right!");
                    break;
                }
                
                if (target.position.y - transform.position.y < 0f && (target.position.x - transform.position.x >= -2f || target.position.x + 2f >= transform.position.x))
                {
                    enemyDirection = EnemyDirection.Down;
                    print("Top!");
                    break;
                }

                if (target.position.y - transform.position.y > 0f && (target.position.x - transform.position.x >= -2f || target.position.x + 2f >= transform.position.x))
                {
                    enemyDirection = EnemyDirection.Up;
                    print("Bottom!");
                    break;
                }
                break;
        }
    }

    
}   
// All good things must come to an end - Geoffrey Chaucer