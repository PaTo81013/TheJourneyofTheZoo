using System;
using System.Collections;
using System.Collections.Generic;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

public class Killstreak : MonoBehaviour
{
    [Header("Lista de rachas (ScriptableObjects)")]
    public List<DataScorestreaks.DataScorestreaks> listaDeRachas = new List<DataScorestreaks.DataScorestreaks>();

    [Header("Iconos en UI (en el mismo orden que listaDeRachas)")]
    public List<Image> listaDeIconos = new List<Image>();

    [Header("Teclas para cada racha")]
    public List<KeyCode> teclasParaRachas = new List<KeyCode>();

    private bool[] _enCooldown;

    private void Awake()
    {
        _enCooldown = new bool[listaDeRachas.Count];
        for (int i = 0; i < _enCooldown.Length; i++)
            _enCooldown[i] = false;
    }

    private void Update()
    {
        for (int i = 0; i < listaDeRachas.Count; i++)
        {
            if (i >= teclasParaRachas.Count) continue; 
            if (_enCooldown[i]) continue;              

            if (Input.GetKeyDown(teclasParaRachas[i]))
            {
                int puntosActuales = ScoreManager.Instance.GetScore();
                int umbral = listaDeRachas[i].cantidadRequerida;

                if (puntosActuales >= umbral)
                {
                   
                    ActivarRacha(i);
                }
                else
                {
                    Debug.Log($"Te faltan {umbral - puntosActuales} puntos para activar la racha #{i + 1}.");
                }
            }
        }
    }

    private void ActivarRacha(int index)
    {
        if (index < listaDeIconos.Count && listaDeIconos[index] != null)
            listaDeIconos[index].enabled = false;
        
        Instantiate(
            listaDeRachas[index].Prefab, 
            Vector3.zero, 
            Quaternion.identity
        );
        
        _enCooldown[index] = true;
        float duracionCD = listaDeRachas[index].cooldown;
        StartCoroutine(CooldownCoroutine(index, duracionCD));
    }

    private IEnumerator CooldownCoroutine(int index, float duracion)
    {
        yield return new WaitForSeconds(duracion);
        
        if (index < listaDeIconos.Count && listaDeIconos[index] != null)
            listaDeIconos[index].enabled = true;

        _enCooldown[index] = false;
    }
}
