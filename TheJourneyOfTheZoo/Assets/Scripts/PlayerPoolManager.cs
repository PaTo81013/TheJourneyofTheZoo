using System.Collections.Generic;
using UnityEngine;

public class PlayerPoolManager : MonoBehaviour
{
    public static PlayerPoolManager Instance { get; private set; }
    
    [SerializeField] private GameObject vfxHitCrit;
    [SerializeField] private GameObject vfxHitNormal;
    private List<GameObject> VFXHitCritList = new List<GameObject>();
    private List<GameObject> VFXHitNormalList = new List<GameObject>();
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
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
