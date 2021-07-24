using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class STSAIController : MonoBehaviour
{
    public Transform target;
    private Rigidbody2D rb;

    public float nextWaypointDistance = 0.1f;

    public Transform childSprite;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;

    public Animator animator;

    public STSGameConstants stsGameConstants;

    private int AIDirection = 0 ; // 0 is down, 1 is up, 2 is left, 3 is right
    private int changeDirection = 0;

    private bool patrolMode = true;

    public Transform[] STSWayPoints;
    private int numWaypoints;
    private int curWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);

        numWaypoints = STSWayPoints.Length;
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            if (patrolMode)
            {
                if (curWaypoint >= numWaypoints) { curWaypoint = 0; }
                float distanceToDestination = Vector2.Distance(rb.position, STSWayPoints[curWaypoint].position);
                if (distanceToDestination < 0.5f)
                {
                    curWaypoint++;
                }
                target = STSWayPoints[curWaypoint];
            }

            seeker.StartPath(transform.position, target.position, OnPathComplete);
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

    void UpdateFacingDirection()
    {
        /*// down, up, left, right
        if (!patrolMode)
        {
            if (AIDirection == 3 || AIDirection == 2)
            {
                if (target.position.y > transform.position.y || target.position.y < transform.position.y)
                {
                    var eulerRot = Quaternion.Euler(0, 0, transform.rotation.z + 3.0f);
                    transform.rotation = eulerRot;
                    childSprite.localRotation = Quaternion.Euler(0, 0, childSprite.rotation.z - 3.0f);
                }

                if (target.position.y < transform.position.y || target.position.y > transform.position.y)
                {
                    var eulerRot = Quaternion.Euler(0, 0, transform.rotation.z + 3.0f);
                    transform.rotation = eulerRot;
                    childSprite.localRotation = Quaternion.Euler(0, 0, childSprite.rotation.z - 3.0f);
                }
            }
        }*/
       
        if (AIDirection == 0)
        {
            var eulerRot = Quaternion.Euler(0, 0, 0);
            transform.rotation = eulerRot;
            childSprite.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (AIDirection == 1)
        {
            var eulerRot = Quaternion.Euler(-180.0f, 0, 0);
            transform.rotation = eulerRot;
            childSprite.localRotation = Quaternion.Euler(180.0f, 0, 0);
        }

        if (AIDirection == 2)
        {
            var eulerRot = Quaternion.Euler(0, 0, -90.0f);
            transform.rotation = eulerRot;
            childSprite.localRotation = Quaternion.Euler(0, 0, 90.0f);
        }

        if (AIDirection == 3)
        {
            var eulerRot = Quaternion.Euler(0, 0, 90.0f);
            transform.rotation = eulerRot;
            childSprite.localRotation = Quaternion.Euler(0, 0, -90.0f);
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
        //Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - new Vector2(transform.position.x, transform.position.y)).normalized;

        Vector2 force = direction * stsGameConstants.AISpeed * Time.deltaTime;
        rb.AddForce(force);


        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                if (AIDirection == 3)
                {
                    changeDirection++;
                }
                else
                {
                    changeDirection = 0;
                }
                AIDirection = 3;
            }
            else
            {
                if (AIDirection == 2)
                {
                    changeDirection++;
                }
                else
                {
                    changeDirection = 0;
                }
                AIDirection = 2;
            }
            animator.SetFloat("right", direction.x);
            animator.SetFloat("up", 0);
        }
        else
        {
            if (direction.y > 0)
            {
                if (AIDirection == 1)
                {
                    changeDirection++;
                }
                else
                {
                    changeDirection = 0;
                }
                AIDirection = 1;
            }
            else
            {
                if (AIDirection == 0)
                {
                    changeDirection++;
                }
                else { changeDirection = 0; }
                AIDirection = 0;
            }
            animator.SetFloat("up", direction.y);
            animator.SetFloat("right", 0);
        }

        if (changeDirection == 5)
        {
            UpdateFacingDirection();
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            patrolMode = false;
            target = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform == target)
        {
            patrolMode = true;
        }
    }
}
