using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private Transform vfxHitCrit;
    [SerializeField] private Transform vfxHitNormal;
    
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BulletTarget>() != null)
        {
            //Hit target
            //Instantiate(vfxHitCrit, transform.position, Quaternion.identity);
            PlayerPoolManager.Instance.TriggerCriticalBulletExplosion(transform.position);
        } else
        {
            //Hit something else
            //Instantiate(vfxHitNormal, transform.position, Quaternion.identity);
            PlayerPoolManager.Instance.TriggerNormalBulletExplosion(transform.position);
        }
        Destroy(gameObject);
    }
}
