using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class EnemyMovementNavMesh : MonoBehaviour
{
    [SerializeField] public DataEnemies.DataEnemies enemy_Data;
    private NavMeshAgent agent;
    private Animator animator;
    [SerializeField] private Transform pathfinding_Target;
    [SerializeField] private float attack_Distance;
    [SerializeField] private GameObject enemy_Attack_Hitbox;
    private float pathfinding_Distance = 0f;
    private bool attackingAnimation = false;
    private bool forcedInPlace = false;
    

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ResetEnemyStatus();
    }

    public void ResetEnemyStatus()
    {
        attackingAnimation = false;
        forcedInPlace = false;
        pathfinding_Distance = 0f;
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
        GettingHitAnimationtTriggerOnCritHit();
        CheckAttackHitBoxStatusAndTurnOff();
    }

    private void TriggerForcedInPlace()
    {
        forcedInPlace = true;
        agent.isStopped = true;
    }

    private void TriggerNoLongerInPlace()
    {
        forcedInPlace = false;
        agent.isStopped = false;
    }

    private void GettingHitAnimationtTriggerOnCritHit()
    {
        animator.SetBool("Attack", false);
        animator.SetTrigger("Hit");
    }
    
    private void EndingGettingHitAnimation()
    {
        TriggerNoLongerInPlace();
        //animator.SetBool("Hit", false);
    }

    private void AttackHitboxON()
    {
        enemy_Attack_Hitbox.SetActive(true);
    }

    private void AttackHitboxOFF()
    {
        enemy_Attack_Hitbox.SetActive(false);
    }

    private void CheckAttackHitBoxStatusAndTurnOff()
    {
        if (enemy_Attack_Hitbox.activeInHierarchy)
        {
            AttackHitboxOFF();
        }
    }
}
