using System;
using UnityEngine;

public class AttackTriggerEnemy : MonoBehaviour
{
    [SerializeField] private DataEnemies.DataEnemies enemy_Data;
    [SerializeField] private GameObject mainEnemyGameObject;
    private float cooldownMelee = 2f;
    private float lastMeleeTime = 0f;
    private int danoMelee = 0;
    void Start()
    {
        lastMeleeTime = 0f;
        cooldownMelee = enemy_Data.cooldownMelee;
        danoMelee = enemy_Data.danoMelee;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Melee
        if (Time.time >= lastMeleeTime + cooldownMelee)
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponent<ThirdPersonShooterController>().TakingDamageFromEnemies(danoMelee, mainEnemyGameObject.transform.position);
                lastMeleeTime = Time.time;
            }
        }
    }
}
