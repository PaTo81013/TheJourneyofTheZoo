using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffectDisablerTimer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    
    void Start()
    {
        StartCoroutine(DisableThisGameObject());
    }

    IEnumerator DisableThisGameObject()
    {
        yield return new WaitForSeconds(0.6f);
        
        gameObject.SetActive(false);

    }

    public void PlayEffectOnce()
    {
        _particleSystem.Play();
        StartCoroutine(DisableThisGameObject());
    }
}
