using System.Collections;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMODUnity;
using UnityEngine.Serialization;

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
    public Transform realTarget;
    public int currentPoint;
    public Transform[] patrolPoints;
    
    private GameObject emptyTargetObject;
    
    // Variable that enables the Chase switchcase to function only if true
    public bool canChase;
    
    // Player's hiding variable
    private bool isThePlayerHiding;
    
    // Speed for Chase switchcase
    private float speed = 700f;
    
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
    private GameObject playerObject;
    private GameObject t;
    private GameObject e;
    private Animator enemySprite;
    private PlayerController playerController;
    private Animator enemyAnimator;
    private Rigidbody2D playerrb2d;
    


    void Start()
    {
        // Getting PlayerController Script
        playerObject = GameObject.Find("RealPlayer");
        t = GameObject.Find("/Enemy/Monster");
        e = GameObject.Find("Enemy");
        playerController = playerObject.GetComponent<PlayerController>();
        enemySprite = t.GetComponent<Animator>();
        
        

        // Getting components
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cc = GetComponent<CircleCollider2D>();
        playerrb2d = playerObject.GetComponent<Rigidbody2D>();
        
        LayerMask mask = LayerMask.GetMask("Player");
        
        // Setting destination to first patrolpoint in array (since currentPoint is set to 0 by default)
        Vector3 patrolDestination = patrolPoints[currentPoint].position;
        currentPoint = 0;
        
        // Using enum state to make Patrol the default state
        state = State.Patrol;
        
        emptyTargetObject = new GameObject();
        emptyTargetObject.SetActive(false);
        
        
        // Repeats mentioned void function with 0f cooldown and .5f seconds between each repetition
        InvokeRepeating("UpdatePatrolPoint", 0f, .5f);
        InvokeRepeating("UpdateSprite", 0f, .1f);
    }

    void UpdatePatrolPoint()
    {
        var target = realTarget;
        if (!playerController.playerEnabled)
        {
            target = emptyTargetObject.transform;
        }
        
        switch (state)
        {
            case State.Dead:
                break;
                // Switch state containing the Patrol logic
                case State.Patrol:
                    Vector3 patrolDestination = patrolPoints[currentPoint].position;
        
                    float distanceToPatrolDestination = Vector3.Distance(transform.position, patrolDestination);
                    if (distanceToPatrolDestination < 1.5f)
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
                    if (seeker.IsDone() && playerController.playerEnabled)
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
        
        if (playerController.playerIsHiding)
            canChase = false;
    }

    public void CummingForYourAss()
    {
        state = State.Chase;
        canChase = true;
        RuntimeManager.PlayOneShot("event:/Monster/sfx_monsterScream");
        print("finalChase");
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
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        int randbitch = Random.Range(1, 3);
        if (playerController.playerEnabled && other.gameObject.CompareTag("Player") && state == State.Chase) 
        {
            
            switch (enemyDirection)
            {
                case EnemyDirection.Down:
                    if (randbitch == 1)
                    {
                        enemySprite.Play("Enemy_KillFromLeft");
                        break;
                    }
                    else if (randbitch == 2)
                    {
                        enemySprite.Play("Enemy_KillFromRight");
                        break;
                    }

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
            state = State.Dead;
            //cc.enabled = false;
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
        Vector2 targetDir = realTarget.position - transform.position;
        Vector2 enemyPosition = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(enemyPosition, targetDir);
        var playerHorizontal = playerrb2d.linearVelocityX;
        var playerVertical = playerrb2d.linearVelocityY;

        /*if (this.gameObject.layer != LayerMask.NameToLayer("Enemy") ||
            this.gameObject.layer != LayerMask.NameToLayer("EnemyReach"))
        {
            print("Bitch you though");
            return;
        }*/
            
        
        if (hit.collider.CompareTag("Player") && (playerHorizontal > 0f || playerVertical > 0f) && state != State.Chase)
        {
            state = State.Chase;
            canChase = true;
            RuntimeManager.PlayOneShot("event:/Monster/sfx_monsterScream");
        }
    
        
        
    }
    
    IEnumerator Wait1SecLOL()
    {
        _LFP = true;
        coRoutineCounter += 1;

        if (coRoutineCounter == 3)
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
                if (realTarget.position.x - transform.position.x > 2f)
                {
                    enemyDirection = EnemyDirection.Right;
                    break;
                }
                
                if (realTarget.position.x + 2f <= transform.position.x)
                {
                    enemyDirection = EnemyDirection.Left;
                    break;
                }
                
                if (realTarget.position.y - transform.position.y < 0f && (realTarget.position.x - transform.position.x >= -2f || realTarget.position.x + 2f >= transform.position.x))
                {
                    enemyDirection = EnemyDirection.Down;
                    break;
                }

                if (realTarget.position.y - transform.position.y > 0f && (realTarget.position.x - transform.position.x >= -2f || realTarget.position.x + 2f >= transform.position.x))
                {
                    enemyDirection = EnemyDirection.Up;
                    break;
                }
                break;
        }
    }

    
}   
// All good things must come to an end - Geoffrey Chaucer