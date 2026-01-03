using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class GoblinAI : MonoBehaviour
{
    [Header("Doelwitten")]
    public Transform player;
    public List<Transform> waypoints;
    public bool isRespawning;

    [Header("Instellingen")]
    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 4.5f;
    public float sightDistance = 10f;
    public float catchDistance = 1.5f; // NIEUW: Afstand om speler te vangen
    public float eyeHeight = 1.0f;
    public LayerMask obstacleMask;

    [Header("Status (Read Only)")]
    public bool canSeePlayer;
    public float lostTimer = 0f;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private Vector3 lastKnownPosition;

    private enum State { Patrolling, Chasing, Searching }
    private State currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Patrolling;
        agent.autoBraking = false;

        if (waypoints.Count > 0)
            agent.SetDestination(waypoints[0].position);
    }

    void Update()
    {
        CheckVision();

        switch (currentState)
        {
            case State.Patrolling:
                PatrolBehavior();
                break;
            case State.Chasing:
                ChaseBehavior();
                break;
            case State.Searching:
                SearchBehavior();
                break;
        }
    }

    void CheckVision()
    {
        if (player == null) return;

        Vector3 origin = transform.position + Vector3.up * eyeHeight;
        Vector3 direction = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= sightDistance)
        {
            if (!Physics.Raycast(origin, direction, distanceToPlayer, obstacleMask))
            {
                canSeePlayer = true;
                lastKnownPosition = player.position;
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else
        {
            canSeePlayer = false;
        }
    }

    void PatrolBehavior()
    {
        agent.speed = patrolSpeed;
        if (canSeePlayer)
        {
            currentState = State.Chasing;
            return;
        }

        if (waypoints.Count == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void ChaseBehavior()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);

        // NIEUW: Check of de goblin dicht genoeg is om de speler te vangen
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= catchDistance)
        {
            CatchPlayer(); 
        }

        if (!canSeePlayer)
        {
            currentState = State.Searching;
            lostTimer = 0f;
        }
    }

    void SearchBehavior()
    {
        agent.SetDestination(lastKnownPosition);

        // In searching mode kunnen we de speler ook vangen als we toevallig tegen hem aan lopen
        if (Vector3.Distance(transform.position, player.position) <= catchDistance)
        {
            CatchPlayer();
        }

        if (canSeePlayer)
        {
            currentState = State.Chasing;
            return;
        }

        lostTimer += Time.deltaTime;
        if (lostTimer >= 5.0f)
        {
            currentState = State.Patrolling;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            isRespawning = false;
        }
    }

    // NIEUW: De functie die alles regelt als de speler gepakt wordt
    void CatchPlayer()
    {
        Debug.Log("GRIJPEN: Speler is gevangen.");

        // Stap 1: Probeer het script direct op de player te vinden
        LivesManager healthScript = player.GetComponent<LivesManager>();

        // Stap 2: Als we het niet vinden, kijk dan eens in de kinderen (children) van de player
        if (healthScript == null)
        {
            healthScript = player.GetComponentInChildren<LivesManager>();
        }

        // Stap 3: Als we het NOG niet vinden, kijk dan bij de ouder (parent)
        if (healthScript == null)
        {
            healthScript = player.GetComponentInParent<LivesManager>();
        }

        // Stap 4: Check het resultaat
        if (healthScript != null && !isRespawning)
        {
            Debug.Log("LivesManager gevonden! Leven eraf halen...");
            healthScript.LoseLife(); // Zorg dat deze functie exact zo heet in je LivesManager
            StartCoroutine(healthScript.RespawnRoutine());
            isRespawning = true;
            

        }


    }   
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightDistance);

        // NIEUW: Laat zien hoe dichtbij hij moet komen om te vangen
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, catchDistance);

        if (canSeePlayer)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.up * eyeHeight, player.position);
        }
    }
}