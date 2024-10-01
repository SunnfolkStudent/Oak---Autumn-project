using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public int currentPoint;
    public Transform[] patrolPoints;
    public float patrolSpeed;
    bool isMovingForwards = true;

    void Start()
    {
        transform.position = patrolPoints[0].position;
        currentPoint = 0;
    }

    void Update()
    {
        Vector3 destination = patrolPoints[currentPoint].position;
        
        transform.position = Vector3.MoveTowards
            (transform.position, destination, patrolSpeed * Time.deltaTime);
        
        float distanceToDestination = Vector3.Distance(transform.position, destination);
        if (distanceToDestination < 0.2f)
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
    }
}
