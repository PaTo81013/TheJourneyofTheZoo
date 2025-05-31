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
    private float forcedInPlaceLastTime = 0f;
    private float forcedInPlaceStunTime = 3f;//0.4f

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //ResetEnemyStatus();
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
        SettingNavMeshTarget();
        if (!forcedInPlace)
        {
            AttackingInPositionPatterns();
        }
        else
        { 
            CheckForcedInPlaceFlagTime();
        }

        if (attackingAnimation)
        {
            transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(pathfinding_Target.position - this.transform.position, Vector3.up).normalized, 0.01f);
        }
    }

    private void SettingNavMeshTarget()
    {
        agent.destination = pathfinding_Target.position;
    }
    
    private void AttackingInPositionPatterns()
    {
        pathfinding_Distance = Vector3.Distance(agent.transform.position, pathfinding_Target.position);
        //Entering attack range
        if (pathfinding_Distance <= attack_Distance)
        {
            //agent.isStopped = true;
            TriggerForcedInPlace();
            attackingAnimation = true;
            animator.SetBool("Attack", true);
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
        if (attackingAnimation)
        {
            attackingAnimation = false;
            animator.SetBool("Attack", false);
        }
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

    private void CheckForcedInPlaceFlagTime()
    {
        if (forcedInPlace)
        {
            if (Time.time >= forcedInPlaceLastTime + forcedInPlaceStunTime)
            {
                TriggerNoLongerInPlace();
            }
        }
        else
        {
            if (!attackingAnimation)
            {
                //agent.destination = pathfinding_Target.position;
                //agent.isStopped = false;
                if (!forcedInPlace)
                { 
                    //Debug.Log("TRIGGERING NO LONGER IN PLACE, attackingAnimation: " + attackingAnimation + ", forcedInPlace: " + forcedInPlace);
                    TriggerNoLongerInPlace();
                }
                //animator.SetBool("Attack", false);
            }
        }
    }
    private void TriggerForcedInPlace()
    {
        //Debug.Log("FORCED IN PLACE - - - -");
        agent.isStopped = true;
        forcedInPlaceLastTime = Time.time;
        forcedInPlace = true;
    }

    private void TriggerNoLongerInPlace()
    {
        //Debug.Log("alowing movement");
        forcedInPlace = false;
        agent.isStopped = false;
    }

    private void GettingHitAnimationtTriggerOnCritHit()
    {
        TurningOffAttackingAnimationStatus();
        animator.SetTrigger("Hit");
        //animator.Play("Capybara Get Hit");
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
