using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private Transform vfxHitCrit;
    [SerializeField] private Transform vfxHitNormal;
    private Vector3 targetPosition;
    private bool hasImpacted;
    private bool criticalHit;
    private bool raycastHit;
    private GameObject enemyGameObjectReached = default;

    public void Setup(Vector3 targetPosition, bool criticalHit, bool raycastHit, GameObject enemyGameObjectReached)
    {
        this.criticalHit = criticalHit;
        this.raycastHit = raycastHit;
        this.targetPosition = targetPosition;
        this.enemyGameObjectReached = enemyGameObjectReached;
        hasImpacted = false;
    }
    
    private void Update()
    {
        float distanceBefore = Vector3.Distance(transform.position, targetPosition);
        
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float moveSpeed = 200f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        
        float distanceAfter = Vector3.Distance(transform.position, targetPosition);

        if (distanceBefore < distanceAfter && raycastHit)
        {
            hasImpacted = true;
            TriggerVFXEffect();
            //Instantiate VFX From pool manager
            /*
            transform.Find("Trail").SetParent(null);
            Destroy(gameObject);
            */
            ExecuteHitBehaviourForTarget();
            this.gameObject.SetActive(false);
        }
    }

    /*
    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 40f;
        bulletRigidbody.linearVelocity = transform.forward * speed;
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (!hasImpacted && !raycastHit)
        {
            if (other.CompareTag("CriticalHit"))
            {
                //Hit target
                //Instantiate(vfxHitCrit, transform.position, Quaternion.identity);
                criticalHit = true;
                TriggerVFXEffect();
            } else if (other.CompareTag("NormalHit") || other.CompareTag("MissHit"))
            {
                //Hit something else
                //Instantiate(vfxHitNormal, transform.position, Quaternion.identity);
                criticalHit = false;
                TriggerVFXEffect();
            }
        
            hasImpacted = true;
            this.gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    private void TriggerVFXEffect()
    {
        if (criticalHit)
        {
            PlayerPoolManager.Instance.TriggerCriticalBulletExplosion(transform.position);
        }
        else
        {
            PlayerPoolManager.Instance.TriggerNormalBulletExplosion(transform.position);
        }
        this.gameObject.SetActive(false);
    }

    private void ExecuteHitBehaviourForTarget()
    {
        if (criticalHit)
        {
            enemyGameObjectReached.GetComponent<EnemyMovementNavMesh>().GettingCriticalHitByPlayer();
        }
        else
        {
            enemyGameObjectReached.GetComponent<EnemyMovementNavMesh>().GettingHitByPlayer();
        }
    }
    
}
