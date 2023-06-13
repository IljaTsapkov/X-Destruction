using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float waitTime = 3;
    public float walkSpeed = 3;
    public float runSpeed = 5;

    public float viewRadius = 50;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1.0f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;
    public Transform playerPosition;
    private float attackDistance = 3f;
    public float hearingDistance = 6f;

    public Transform[] waypoints;
    private int currentWaypointIndex;

    private Vector3 seenPlayerPosition;

    private float startWaitTime;
    private bool isPlayerInRange;
    private bool isPatroling;

    public GameObject rightFist;
    public GameObject leftFist;

    Animator animator;

    void Start()
    {
        playerPosition = FindObjectOfType<Player>().transform;
        seenPlayerPosition = Vector3.zero;
        isPatroling = true;
        isPlayerInRange = false;
        startWaitTime = waitTime; //  Set the wait time variable that will change

        currentWaypointIndex = 0; //  Set the initial waypoint
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        animator = GetComponent<Animator>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = walkSpeed; //  Set the navemesh speed with the normal speed of the enemy
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position); //  Set the destination to the first waypoint
    }

    private void Update()
    {
        AISight(); //  Check whether or not the player is in the enemy's field of vision

        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);
        if (distanceToPlayer <= hearingDistance)
        {
            isPatroling = false;
            isPlayerInRange = true;
        }

        if (!isPatroling)
        {
            animator.SetBool("isAware", true);
            Chasing();
        }
        else
        {
            animator.SetBool("isAware", false);
            Patroling();
        }

        Attacking();
    }

    private void Attacking()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);

        if (distanceToPlayer <= attackDistance) // check if the distance to the player is less than or equal to the attack distance
        {
            animator.SetInteger("AttackIndex", Random.Range(0, 2));
            animator.SetBool("isAttacking", true);
        }
    }

    private void Patroling()
    {
        // The enemy is patroling

        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (startWaitTime <= 0)
            {
                animator.SetBool("isChilling", false);
                NextPoint();
                Move(walkSpeed);
                startWaitTime = waitTime;
            }
            else
            {
                animator.SetBool("isChilling", true);
                Stop();
                startWaitTime -= Time.deltaTime;
            }
        }
    }

    private void Chasing() {
        // The enemy is chasing the player

        animator.SetBool("isAttacking", false);
        animator.SetBool("isChillAfterRun", false);
        Move(runSpeed);
        navMeshAgent.SetDestination(seenPlayerPosition); // Set the destination of the enemy to the player location

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) // Control if the enemy arrives to the player location
        {
            if (startWaitTime <= 0 && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f) { // Check if the enemy is not near to the player, returns to patrol after the wait time delay
                isPatroling = true;
                Move(walkSpeed);
                startWaitTime = waitTime;
                navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
            } else {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f) // Wait if the current position is not the player position
                    animator.SetBool("isChillAfterRun", true);
                Stop();
                startWaitTime -= Time.deltaTime;
            }
        }
    }

    public void NextPoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    public void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    public void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    public void CantMove()
    {
        walkSpeed = 0;
        runSpeed = 0;
    }

    public void CanMove()
    {
        walkSpeed = 3;
        runSpeed = 5;
    }

    public void ActivateRightFistCollider()
    {
        rightFist.GetComponent<Collider>().enabled = true;
    }

    public void DeactivateRightFistCollider()
    {
        rightFist.GetComponent<Collider>().enabled = false;
    }

    public void ActivateLeftFistCollider()
    {
        leftFist.GetComponent<Collider>().enabled = true;
    }

    public void DeactivateLeftFistCollider()
    {
        leftFist.GetComponent<Collider>().enabled = false;
    }

    public void SetIsPatroling(bool value)
    {
        isPatroling = value;
    }

    public void SetIsPlayerInRange(bool value)
    {
        isPlayerInRange = value;
    }

    private void AISight()
    {
        Collider[] playerInRange = Physics.OverlapSphere(
            transform.position,
            viewRadius,
            playerMask
        );

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 direction = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, direction) < viewAngle / 2)
            {
                float distance = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, direction, distance, obstacleMask))
                {
                    isPlayerInRange = true;
                    isPatroling = false;
                }
                else
                {
                    isPlayerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                isPlayerInRange = false;
            }
            if (isPlayerInRange)
            {
                seenPlayerPosition = player.transform.position;
            }
        }
    }
}
