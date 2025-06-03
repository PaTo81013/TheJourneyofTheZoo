using System;
using UnityEngine;

public class EnemySpawnerPoolManager : MonoBehaviour
{
    public static EnemySpawnerPoolManager Instance { get; private set; }
    
    [Header("Enemigos Nivel 1")]
    [SerializeField] private GameObject perico;
    [SerializeField] private GameObject tucan;
    [SerializeField] private GameObject pollo;
    [SerializeField] private GameObject piton;
    
    [Header("Enemigos Nivel 2")]
    [SerializeField] private GameObject capybara;
    [SerializeField] private GameObject pantera;
    [SerializeField] private GameObject tigre;
    
    [Header("Enemigos Nivel 3")]
    [SerializeField] private GameObject chango;
    [SerializeField] private GameObject orangutan;
    [SerializeField] private GameObject gorila;
    
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

    private void Start()
    {
        
    }
}
