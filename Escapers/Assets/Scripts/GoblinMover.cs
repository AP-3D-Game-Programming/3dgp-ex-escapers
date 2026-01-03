using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class GoblinAI : MonoBehaviour
{
    [Header("Doelwitten")]
    public Transform player;            // Sleep de speler hierin
    public List<Transform> waypoints;   // Sleep hier lege GameObjects in als patrouillepunten

    [Header("Instellingen")]
    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 4.5f;
    public float sightDistance = 10f;   // Hoe ver kan de goblin kijken?
    public float eyeHeight = 1.0f;      // Hoogte van de ogen (zodat raycast niet over de vloer gaat)
    public LayerMask obstacleMask;      // Welke lagen blokkeren het zicht (bv. Muren)?

    [Header("Status (Read Only)")]
    public bool canSeePlayer;
    public float lostTimer = 0f;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private Vector3 lastKnownPosition;

    // We definiëren de statussen
    private enum State { Patrolling, Chasing, Searching }
    private State currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Start met patrouilleren
        currentState = State.Patrolling;

        // Zorg dat de agent niet stopt als hij wisselt van punt
        agent.autoBraking = false;

        if (waypoints.Count > 0)
            agent.SetDestination(waypoints[0].position);
    }

    void Update()
    {
        // 1. Check of we de speler kunnen zien
        CheckVision();

        // 2. Beheer de statussen
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

        // Check 1: Is de speler binnen bereik?
        if (distanceToPlayer <= sightDistance)
        {
            // Check 2: Raycast (Is er een muur?)
            // We sturen een straal naar de speler. Als we IETS raken dat in de obstacleMask zit, zien we de speler niet.
            if (!Physics.Raycast(origin, direction, distanceToPlayer, obstacleMask))
            {
                canSeePlayer = true;
                lastKnownPosition = player.position; // Onthoud waar we hem zagen
            }
            else
            {
                canSeePlayer = false; // Muur in de weg
            }
        }
        else
        {
            canSeePlayer = false; // Te ver weg
        }
    }

    void PatrolBehavior()
    {
        agent.speed = patrolSpeed;

        // Als we de speler zien -> Switch naar Chasing
        if (canSeePlayer)
        {
            currentState = State.Chasing;
            return;
        }

        // Geen waypoints? Doe niets.
        if (waypoints.Count == 0) return;

        // Check of we dichtbij het huidige waypoint zijn
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // Ga naar het volgende punt
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void ChaseBehavior()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);

        // Als we de speler NIET meer zien
        if (!canSeePlayer)
        {
            // Switch naar Searching (de 5 seconden timer begint)
            currentState = State.Searching;
            lostTimer = 0f;
        }
    }

    void SearchBehavior()
    {
        // Blijf naar de laatst bekende positie rennen
        agent.SetDestination(lastKnownPosition);

        // Als we de speler weer zien -> Terug naar Chasing
        if (canSeePlayer)
        {
            currentState = State.Chasing;
            return;
        }

        // Timer laten lopen
        lostTimer += Time.deltaTime;

        // Als 5 seconden voorbij zijn -> Terug naar Patrolling
        if (lostTimer >= 5.0f)
        {
            currentState = State.Patrolling;
            // Ga direct naar het dichtstbijzijnde waypoint of vervolg route
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    // Visuele hulp in de Scene view om de Raycast en bereik te zien
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightDistance);

        if (canSeePlayer)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.up * eyeHeight, player.position);
        }
    }
}