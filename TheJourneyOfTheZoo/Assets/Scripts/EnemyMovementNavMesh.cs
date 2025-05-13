using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class EnemyMovementNavMesh : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    [SerializeField] private Transform pathfinding_Target;
    [SerializeField] private float attack_Distance;
    private float pathfinding_Distance = 0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AnimationIdleOrRunBehaviour();
        MovingThroughPathfinding();
    }

    private void MovingThroughPathfinding()
    {
        pathfinding_Distance = Vector3.Distance(agent.transform.position, pathfinding_Target.position);
        if (pathfinding_Distance < attack_Distance)
        {
            agent.isStopped = true;
            animator.SetBool("Attack", true);
        }
        else
        {
            agent.isStopped = false;
            animator.SetBool("Attack", false);
            agent.destination = pathfinding_Target.position;
        }
    }

    private void AnimationIdleOrRunBehaviour()
    {
        if (agent.velocity.magnitude != 0f)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }
    }
    
}
