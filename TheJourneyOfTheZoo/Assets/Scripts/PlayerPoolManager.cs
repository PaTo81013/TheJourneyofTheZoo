using System.Collections.Generic;
using UnityEngine;

public class PlayerPoolManager : MonoBehaviour
{
    public static PlayerPoolManager Instance { get; private set; }
    
    [SerializeField] private GameObject vfxHitCrit;
    [SerializeField] private GameObject vfxHitNormal;
    [SerializeField] private GameObject bulletPrefab;
    private List<GameObject> VFXHitCritList = new List<GameObject>();
    private List<GameObject> VFXHitNormalList = new List<GameObject>();
    private List<GameObject> BulletList = new List<GameObject>();
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void InstantiateBulletForShoot(Vector3 positionToBeSpawned, Vector3 aimingDirection, Vector3 targetPosition, bool criticalHit, bool raycastHit, GameObject reachedGameObject)
    {
        //Verificamos si no existe un game object disponible en la jerarquia
        for (int i = 0; i < BulletList.Count; i++)
        {
            if (!BulletList[i].activeInHierarchy)
            {
                BulletList[i].transform.rotation = Quaternion.LookRotation(aimingDirection, Vector3.up); 
                BulletList[i].transform.position = positionToBeSpawned;
                BulletList[i].GetComponent<BulletProjectile>().Setup(targetPosition, criticalHit, raycastHit, reachedGameObject);
                BulletList[i].SetActive(true);
                return;
            }
        }
        
        //Si no hay game objects disponibles, entonces instanciamos uno y lo agregamos al pool
        GameObject newBullet = Instantiate(bulletPrefab, positionToBeSpawned, Quaternion.LookRotation(aimingDirection, Vector3.up));
        newBullet.GetComponent<BulletProjectile>().Setup(targetPosition, criticalHit, raycastHit, reachedGameObject);
        //BulletList.Add(Instantiate(bulletPrefab, positionToBeSpawned, Quaternion.LookRotation(aimingDirection, Vector3.up)));
        BulletList.Add(newBullet);
    }

    public void TriggerNormalBulletExplosion(Vector3 positionToBeSpawned)
    {
        //Verificamos si no existe un game object disponible en la jerarquia
        for (int i = 0; i < VFXHitNormalList.Count; i++)
        {
            if (!VFXHitNormalList[i].activeInHierarchy)
            {
                VFXHitNormalList[i].transform.position = positionToBeSpawned;
                VFXHitNormalList[i].SetActive(true);
                VFXHitNormalList[i].GetComponent<SpecialEffectDisablerTimer>().PlayEffectOnce();
                return;
            }
        }

        //Si no hay game objects disponibles, entonces instanciamos uno y lo agregamos al pool
        VFXHitNormalList.Add(Instantiate(vfxHitNormal, positionToBeSpawned, Quaternion.identity));
    }
    
    public void TriggerCriticalBulletExplosion(Vector3 positionToBeSpawned)
    {
        //Verificamos si no existe un game object disponible en la jerarquia
        for (int i = 0; i < VFXHitCritList.Count; i++)
        {
            if (!VFXHitCritList[i].activeInHierarchy)
            {
                VFXHitCritList[i].transform.position = positionToBeSpawned;
                VFXHitCritList[i].SetActive(true);
                VFXHitCritList[i].GetComponent<SpecialEffectDisablerTimer>().PlayEffectOnce();
                return;
            }
        }

        //Si no hay game objects disponibles, entonces instanciamos uno y lo agregamos al pool
        VFXHitCritList.Add(Instantiate(vfxHitCrit, positionToBeSpawned, Quaternion.identity));
    }
}
