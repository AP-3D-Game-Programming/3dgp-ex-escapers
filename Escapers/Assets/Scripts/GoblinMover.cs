using UnityEngine;
using UnityEngine.AI; // This is required to use NavMesh features

[RequireComponent(typeof(NavMeshAgent))] // Ensures the object has an Agent component
public class SimpleFollowAgent : MonoBehaviour
{
    [Header("Targeting")]
    public Transform target; // Drag your Player object here in the Inspector

    [Header("Movement Settings")]
    [Range(0.1f, 5.0f)]
    public float agentSpeed = 3.5f; // Defaulted to a slower walking pace

    private NavMeshAgent agent;

    void Start()
    {
        // Get reference to the component
        agent = GetComponent<NavMeshAgent>();

        // Apply the speed setting
        agent.speed = agentSpeed;
    }

    void Update()
    {
        // If we have a target, tell the agent to go there
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}