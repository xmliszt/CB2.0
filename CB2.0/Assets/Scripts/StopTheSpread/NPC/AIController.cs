using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIController : MonoBehaviour
{
    private Rigidbody2D AIBody;    

    public STSGameConstants stsGameConstants;

    // number of waypoints and directions must be same!
    // final waypoint should be starting position

    public Transform startingPosition;

    [Header("Waypoints")]
    public Transform[] wayPoints;
    public String[] directions;

    private int numberWayPoints;
    private int currentWayPointCounter = 0;

    private Vector3 finalInputMovement = Vector3.zero; // computed with deltaTime and speed
    private Vector3 rawInputMovement = Vector3.zero;

    private int AICurrentDirection = 0; // 1,2,3,4 relates to up,down,left,right, 0 is not moving state

    private float desiredLocation;
    private float distanceFromWayPoint;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        numberWayPoints = wayPoints.Length;
        UpdateAIDirection();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        finalInputMovement = rawInputMovement * Time.deltaTime * stsGameConstants.AISpeed;
        transform.Translate(finalInputMovement, Space.World);
        animator.SetFloat("right", rawInputMovement.x);
        animator.SetFloat("up", rawInputMovement.y);

        CalculateDistance();
        
        if(Mathf.Abs(distanceFromWayPoint) < 0.05f)
        {
            if(currentWayPointCounter < numberWayPoints - 1)
            {
                currentWayPointCounter++;
            }
            else
            {
                currentWayPointCounter = 0;
            }
            
            UpdateAIDirection();
        }
    }

    private void UpdateAIDirection()
    {
        if (directions[currentWayPointCounter] == "Up" || directions[currentWayPointCounter] == "up")
        {
            AICurrentDirection = 1;
            desiredLocation = wayPoints[currentWayPointCounter].position.y;
            rawInputMovement = new Vector3(0, 1.0f, 0);
        }

        if (directions[currentWayPointCounter] == "Down" || directions[currentWayPointCounter] == "down")
        {
            AICurrentDirection = 2;
            desiredLocation = wayPoints[currentWayPointCounter].position.y;
            rawInputMovement = new Vector3(0, -1.0f, 0);
        }

        if (directions[currentWayPointCounter] == "Left" || directions[currentWayPointCounter] == "left")
        {
            AICurrentDirection = 3;
            desiredLocation = wayPoints[currentWayPointCounter].position.x;
            rawInputMovement = new Vector3(-1.0f, 0, 0);
        }

        if (directions[currentWayPointCounter] == "Right" || directions[currentWayPointCounter] == "right")
        {
            AICurrentDirection = 4;
            desiredLocation = wayPoints[currentWayPointCounter].position.x;
            rawInputMovement = new Vector3(1.0f, 0, 0);
        }
        
    }

    private void CalculateDistance()
    {
        if (AICurrentDirection == 1 || AICurrentDirection == 2)
        {
            distanceFromWayPoint = desiredLocation - gameObject.transform.position.y;
        }
        else
        {
            distanceFromWayPoint = desiredLocation - gameObject.transform.position.x;
        }

        //Debug.Log("Distance from way point: " + distanceFromWayPoint);
    }
}
