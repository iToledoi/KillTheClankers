using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIMovementNavMesh : MonoBehaviour
{
    private NavMeshAgent agent;
    private AIData aiData;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        aiData = GetComponent<AIData>();
    }

    private void Update()
    {
        // If a target exists, move toward it
        if (aiData != null && aiData.targets != null && aiData.targets.Count > 0)
        {
            Transform target = aiData.targets[0];
            if (target != null)
            {
                agent.SetDestination(target.position);
            }
        }
        else
        {
            // No targets detected stop moving
            if (agent.hasPath)
                agent.ResetPath();
        }
    }
}
