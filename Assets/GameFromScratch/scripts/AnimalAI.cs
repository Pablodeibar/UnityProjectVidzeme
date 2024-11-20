using UnityEngine;
using UnityEngine.AI;

public class AnimalAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public float walkRadius = 10f; // Radius within which to find random destinations

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToRandomPoint();
    }

    void Update()
    {
        // Move to a new random point when the agent reaches its destination
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
        {
            MoveToRandomPoint();
        }
    }

    void MoveToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1))
        {
            Vector3 finalPosition = hit.position;
            agent.SetDestination(finalPosition);
        }
    }
}
