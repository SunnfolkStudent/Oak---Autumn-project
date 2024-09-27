using System;
using Pathfinding;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{

    public Transform[] patrolPoints;
    
  public Transform target;

    public float speed = 200f;
    
    public bool _canChase = false;

    public float nextWaypointDistance = 3f;

    private Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        
        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }

    void UpdatePath()
    {
        /*if (!_canChase)
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(rb.position, patrolPoints[0].position, OnPathComplete);
            }
        }*/
        if (_canChase)
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(rb.position, target.position, OnPathComplete);
            }
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

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
        Vector2 force = direction * (speed * Time.deltaTime);
        rb.AddForce(force);
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        
        
    }
}
