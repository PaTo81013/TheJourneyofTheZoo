using System;
using Scenes;
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
    private float forcedInPlaceStunTime = 3f;
    private int enemyCurrentLife = 0;
    private bool enemyIsAlive = true;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        pathfinding_Target = GameObject.FindGameObjectWithTag("Player").transform;
        FillEnemyData();
        enemyIsAlive = true;
        animator.SetBool("Death", false);
        agent.isStopped = false;
        //ResetEnemyStatus();
    }

    public void ResetEnemyStatus()
    {
        attackingAnimation = false;
        forcedInPlace = false;
        pathfinding_Distance = 0f;
        FillEnemyData();
        enemyIsAlive = true;
        animator.SetBool("Death", false);
        if (this.gameObject.activeInHierarchy)
        {
            agent.isStopped = false;
        }
    }

    void Update()
    {
        if (enemyIsAlive)
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
        else
        {
            agent.isStopped = true;
            animator.SetBool("Death", true);
        }
    }

    private void SettingNavMeshTarget()
    {
        if (enemyIsAlive && pathfinding_Target != null)
        {
            agent.destination = pathfinding_Target.position;
        }
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
        if (!enemyIsAlive)
        {
            return;
        }
        ScoreManager.Instance.IncreaseScore(enemy_Data.puntos);
        
        DecreaseEnemyCurrentLife(10);
        CheckEnemyAliveStatus();
    }
    
    public void GettingCriticalHitByPlayer()
    {
        if (!enemyIsAlive)
        {
            return;
        }
        ScoreManager.Instance.IncreaseScore(enemy_Data.puntos + enemy_Data.bonusCritico);
        TriggerForcedInPlace();
        GettingHitAnimationtTriggerOnCritHit();
        CheckAttackHitBoxStatusAndTurnOff();
        if (ScoreManager.Instance.GetBananaYagaStatus())
        {
            DecreaseEnemyCurrentLife(30);
        }
        else
        {
            DecreaseEnemyCurrentLife(20);
        }

        CheckEnemyAliveStatus();
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

    private void FillEnemyData()
    {
        enemyCurrentLife = enemy_Data.vida;
        agent.speed = (float)enemy_Data.movimiento / 10f;
    }

    private void DecreaseEnemyCurrentLife(int damageAmount)
    {
        enemyCurrentLife -= damageAmount;
    }

    private void CheckEnemyAliveStatus()
    {
        if (enemyCurrentLife <= 0)
        {
            enemyIsAlive = false;
            agent.isStopped = true;
            animator.SetBool("Death", true);
        }
    }

    public void TeleportAgentToSpot(Vector3 telportedPosition)
    {
        agent.Warp(telportedPosition);
    }

    public void DisableEnemyInHierarchy()
    {
        this.gameObject.SetActive(false);
    }
}
