using System.Collections;
using UnityEngine;
using Pathfinding;
using UnityEditor;
using UnityEngine.SceneManagement;

public class StateEnemyAI : MonoBehaviour
{
    // Creating enum with both enemy states; Patrol and Chase'
    private enum State
    {
        Patrol, 
        Chase
    }
    
    // Public usual variables
    public Transform target;
    public int currentPoint;
    public Transform[] patrolPoints;
    
    
    // Variable that enables the Chase switchcase to function only if true
    public bool canChase;
    
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
    

    void Start()
    {
        
        // Getting components
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cc = GetComponent<CircleCollider2D>();
        
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
            default:
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

    void FixedUpdate()
    {
        switch (state)
        {
            default:
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
    }

    // WIP WIP WIP WIP WIP 
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.Play("Enemy_KillFromBehind");
            cc.enabled = false;
        }
    }
    

    
    // Handles the raycasting when inside of enemy hearing radius
    private void OnTriggerStay2D(Collider2D other)
    {
        
        
        Vector2 targetDir = target.position - transform.position;
        Vector2 enemyPosition = transform.position;
        
        
        RaycastHit2D hit = Physics2D.Raycast(enemyPosition, targetDir);

        if (hit.collider.CompareTag("Player"))
        {
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
        
        /*Vector2 targetDir = target.position - transform.position;
        Vector2 enemyPosition = transform.position;
                                
        RaycastHit2D hit = Physics2D.Raycast(enemyPosition, targetDir, 2f);

        if (hit.collider.CompareTag("Player"))
        {
            print("Hit!!!");
            state = State.Chase;
            canChase = true;
        }*/
        
        yield return new WaitForSeconds(1);
        _LFP = false;
    }

    private void UpdateSprite()
    {
        switch (state)
        {
            case State.Patrol:
                break;
            case State.Chase:
                // Time for some sprite location logic bullshit!
                // If the target is above, and within + or - 2 to the sides, moving downward sprite
                // If the target is below, and within + or -2 to the sides, moving upward sprite
                // If the target is to the left more than 2, moving right sprite
                // If the target is to the right more than 2, moving left sprite
                if (target.position.x - transform.position.x > 2f)
                {
                    print("Left!");
                    break;
                }
                
                if (target.position.x + 2f <= transform.position.x)
                {
                    print("Right!");
                    break;
                }
                
                if (target.position.y - transform.position.y < 0f && (target.position.x - transform.position.x >= -2f || target.position.x + 2f >= transform.position.x))
                {
                    print("Top!");
                    break;
                }

                if (target.position.y - transform.position.y > 0f && (target.position.x - transform.position.x >= -2f || target.position.x + 2f >= transform.position.x))
                {
                    print("Bottom!");
                    break;
                }
                break;
        }
    }

    // All good things must come to an end - Geoffrey Chaucer
    private void GameOver()
    {
        // SceneManager.LoadScene("Game Over scene name goes here");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}