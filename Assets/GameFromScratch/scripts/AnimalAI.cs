using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AnimalAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public float walkRadius = 20f; // Radius within which to find random destinations
    public float walkSpeed = 3.5f; // Speed for walking
    public float runSpeed = 7f; // Speed for running
    public float runProbability = 0.5f; // Probability of running instead of walking
    public float changeInterval = 5f; // Time interval to force change destination
    public int maxNavMeshRetryAttempts = 10; // Max attempts to place on NavMesh
    public float retryDelay = 0.1f; // Delay between retry attempts

    private float timer; // Timer to track time passed

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(EnsureNavMeshPlacement());
    }

    IEnumerator EnsureNavMeshPlacement()
    {
        int attempt = 0;
        while (!agent.isOnNavMesh && attempt < maxNavMeshRetryAttempts)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, walkRadius, NavMesh.AllAreas))
            {
                transform.position = hit.position;
                agent.Warp(hit.position); // Ensures agent is placed correctly
            }
            attempt++;
            yield return new WaitForSeconds(retryDelay);
        }

        if (agent.isOnNavMesh)
        {
            MoveToRandomPoint();
        }
        else
        {
            Debug.LogError("NavMeshAgent could not be placed on the NavMesh after several attempts.");
        }
    }

    void Update()
    {
        if (!agent.isOnNavMesh)
        {
            return;
        }

        // Increment the timer
        timer += Time.deltaTime;

        // Continuously find new random points within the NavMesh
        if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance || agent.remainingDistance == 0f) || timer >= changeInterval)
        {
            MoveToRandomPoint();
            timer = 0; // Reset timer
        }
    }

    void MoveToRandomPoint()
    {
        if (!agent.isOnNavMesh)
        {
            return;
        }

        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas))
        {
            Vector3 finalPosition = hit.position;

            // Randomly decide whether to walk or run
            if (Random.value < runProbability)
            {
                agent.speed = runSpeed;
                //Debug.Log("Running to: " + finalPosition);
            }
            else
            {
                agent.speed = walkSpeed;
                //Debug.Log("Walking to: " + finalPosition);
            }

            agent.SetDestination(finalPosition);
        }
        else
        {
            Debug.LogWarning("Failed to find a valid NavMesh point within radius.");
        }
    }
}
