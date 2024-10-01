using UnityEngine;
using Pathfinding;

public class EnemyPatrolTest : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase
    }
    
    public Transform target;
    public int currentPoint;
    public Transform[] patrolPoints;
    public float patrolSpeed;
    bool isMovingForwards = true;
    public float nextWaypointDistance = 3f;
    
    //Raycast Testing
    private float maxRange = 5f;
    
   

    private State state;

    private Path path;
    public int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    private Rigidbody2D rb;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        Vector3 patrolDestination = patrolPoints[currentPoint].position;
        currentPoint = 0;
        state = State.Patrol;
        
        
        InvokeRepeating("UpdatePatrolPoint", 0f, .5f);
    }

    void UpdatePatrolPoint()
    {
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
        LayerMask mask = LayerMask.GetMask("Player");
        switch (state)
        {
            default:
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
        
                    Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                    Vector2 force = direction * (patrolSpeed * Time.deltaTime);
        
                    rb.AddForce(force);
        
                    float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                    if (distance < nextWaypointDistance)
                    {
                        currentWaypoint++;
                    }

                    break;
            case State.Chase:   
                break;
        }
        
        
        
        
        /*
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
        
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * (patrolSpeed * Time.deltaTime);
        
        rb.AddForce(force);
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }*/
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
        
        Vector2 targetDir = target.position - transform.position;
        Vector2 enemyPosition = transform.position;
        
        
        RaycastHit2D hit = Physics2D.Raycast(enemyPosition, targetDir);

        if (hit.collider.CompareTag("Player"))
        {
            print("Hit!!!");
        }
    }
        
    
}