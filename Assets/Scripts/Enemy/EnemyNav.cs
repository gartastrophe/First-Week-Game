using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    public enum States
    {
        Idle,
        Patrol,
        Chase,
        Searching
    }

    public States currentState;

    Animator anim;

    FieldOfView fieldOfView;

    public GameObject player;
    public float attackDistance = 1f;
    float distanceToPlayer;

    private JumpScareController jumpScareController;
    bool playerJumpscared;
    LevelManager levelManager;

    //int currentDestinationIndex = 0;

    public NavMeshAgent agent;
    public float range;
    public Transform centrePoint;

    float idleStateTimer;
    float searchingStateTimer;

    public float maxIdleTime = 5f;
    public float maxSearchTime = 10f;

    private Vector3 lastKnownPlayerPosition;
    public float searchRange = 4;

    void Start()
    {
        gameObject.SetActive(true);
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        jumpScareController = player.GetComponent<JumpScareController>();
        playerJumpscared = false;
        //wanderPoints = GameObject.FindGameObjectsWithTag("WayPoint");
        anim = GetComponent<Animator>();
        fieldOfView = GetComponentInParent<FieldOfView>();

        agent = GetComponent<NavMeshAgent>();

        Initialize();
    }

    void Update()
    {
        if (levelManager.gameOverTriggered)
        {
            gameObject.SetActive(false);
            playerJumpscared = true;
        }
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        switch (currentState)
        {
            case States.Patrol:
                UpdatePatrolState();
                break;
            case States.Chase:
                UpdateChaseState();
                break;
            case States.Searching:
                UpdateSearchState();
                break;
            case States.Idle:
                UpdateIdleState();
                break;
        }
    }

    private void Initialize()
    {
        currentState = States.Patrol;
    }

    //RandomPoint() and PATROL STATE CODE TAKEN FROM JONDEVTUTORIAL GITHUB https://github.com/JonDevTutorial/RandomNavMeshMovement/blob/main/RandomMovement.cs
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    void UpdatePatrolState()
    {
        anim.SetInteger("state", 1);

        //https://github.com/JonDevTutorial/RandomNavMeshMovement/blob/main/RandomMovement.cs
        if (agent.remainingDistance <= agent.stoppingDistance) 
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) 
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.SetDestination(point);
                currentState = States.Idle;
                idleStateTimer = maxIdleTime;
            }
        }

        if (fieldOfView.IsPlayerInClearFov())
        {
            currentState = States.Chase;
        }

    }

    void UpdateIdleState()
    {
        //Debug.Log(idleStateTimer);
        anim.SetInteger("state", 0);

        //i asked chatgpt how to stop the ai from moving
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        idleStateTimer -= Time.deltaTime;

        if (idleStateTimer <= 0)
        {
            agent.isStopped = false;
            currentState = States.Patrol;
        } 
        else if (fieldOfView.IsPlayerInClearFov())
        {
            agent.isStopped = false;
            currentState = States.Chase;
        }
    }


    void UpdateChaseState()
    {
        anim.SetInteger("state", 2);

        if (distanceToPlayer <= attackDistance && !PlayerInteract.currentlyHiding) // added check for if player is hiding
        {
            if (!playerJumpscared)
            {
                levelManager.timerDisabled = true;
                playerJumpscared = true;
                jumpScareController.TriggerJumpScare();
                gameObject.SetActive(false);
                Invoke("RestartLevel", 2);
            }
        }
        else if (!fieldOfView.IsPlayerInClearFov())
        {
            lastKnownPlayerPosition = player.transform.position;
            searchingStateTimer = maxSearchTime;
            currentState = States.Searching;
        }

        agent.SetDestination(player.transform.position);
    }

    void RestartLevel()
    {
        levelManager.ReloadCurrentLevel();
    }
    void UpdateSearchState()
    {
        Debug.Log(searchingStateTimer);
        if (distanceToPlayer <= attackDistance && !PlayerInteract.currentlyHiding) // added check for if player is hiding
        {
            if (!playerJumpscared)
            {
                levelManager.timerDisabled = true;
                playerJumpscared = true;
                jumpScareController.TriggerJumpScare();
                gameObject.SetActive(false);
                Invoke("RestartLevel", 2);
            }
        }
        searchingStateTimer -= Time.deltaTime;
        if (fieldOfView.IsPlayerInClearFov())
        {
            currentState = States.Chase;
        }
        else if (searchingStateTimer <= 0)
        {
            currentState = States.Patrol;
        }

        //chatgpt helped me create better search behavior
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(lastKnownPlayerPosition, searchRange, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.red, 1.0f);
                agent.SetDestination(point);
            }
        }
    }


    //Code from chatgpt to help me visualize jumpscare distance
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set the color of the gizmo
        Gizmos.DrawWireSphere(transform.position, attackDistance); // Draw the sphere
    }
}