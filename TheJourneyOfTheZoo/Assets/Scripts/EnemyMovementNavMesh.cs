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
    private bool attackingAnimation = false;
    private bool forcedInPlace = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AnimationIdleOrRunBehaviour();
        if (!forcedInPlace)
        {
            MovingThroughPathfinding();
        }
    }

    private void MovingThroughPathfinding()
    {
        pathfinding_Distance = Vector3.Distance(agent.transform.position, pathfinding_Target.position);
        if (pathfinding_Distance <= attack_Distance)
        {
            agent.isStopped = true;
            attackingAnimation = true;
            animator.SetBool("Attack", true);
        }
        else
        {
            if (!attackingAnimation)
            {
                agent.isStopped = false;
                //animator.SetBool("Attack", false);
                agent.destination = pathfinding_Target.position;
            }
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

    private void TurningOffAttackingAnimationStatus()
    {
        attackingAnimation = false;
        animator.SetBool("Attack", false);
    }

    public void GettingHitByPlayer()
    {
        //TriggerForcedInPlace();
    }
    
    public void GettingCriticalHitByPlayer()
    {
        TriggerForcedInPlace();
        GettingHitAnimationtTrigger();
    }

    private void TriggerForcedInPlace()
    {
        forcedInPlace = true;
    }

    private void TriggerNoLongerInPlace()
    {
        forcedInPlace = false;
    }

    private void GettingHitAnimationtTrigger()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("Hit", true);
    }
    
    private void EndingGettingHitAnimation()
    {
        TriggerNoLongerInPlace();
        animator.SetBool("Hit", false);
    }
}
