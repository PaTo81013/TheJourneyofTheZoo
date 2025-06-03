using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerPoolManager : MonoBehaviour
{
    public static EnemySpawnerPoolManager Instance { get; private set; }
    
    [Header("Enemigos Nivel 1, Index 0 - 3")]
    [SerializeField] private GameObject perico;
    [SerializeField] private GameObject tucan;
    [SerializeField] private GameObject pollo;
    [SerializeField] private GameObject piton;
    
    [Header("Enemigos Nivel, Index 4 - 6")]
    [SerializeField] private GameObject capybara;
    [SerializeField] private GameObject pantera;
    [SerializeField] private GameObject tigre;
    
    [Header("Enemigos Nivel 3, Index 7 - 9")]
    [SerializeField] private GameObject chango;
    [SerializeField] private GameObject orangutan;
    [SerializeField] private GameObject gorila;
    
    [Header("Limites del nivel")]
    [SerializeField] private GameObject pared1;
    [SerializeField] private GameObject pared2;
    [SerializeField] private GameObject pared3;
    [SerializeField] private GameObject pared4;
    
    [Header("Referencia al Jugador")]
    [SerializeField] private GameObject jugador;
    
    [Header("Nivel Actual")]
    [SerializeField] private int currentLevel = 1;
    
    private List<GameObject> pericoLista = new List<GameObject>();
    private List<GameObject> tucanLista = new List<GameObject>();
    private List<GameObject> polloLista = new List<GameObject>();
    private List<GameObject> pitonLista = new List<GameObject>();
    private List<GameObject> capybaraLista = new List<GameObject>();
    private List<GameObject> panteraLista = new List<GameObject>();
    private List<GameObject> tigreLista = new List<GameObject>();
    private List<GameObject> changoLista = new List<GameObject>();
    private List<GameObject> orangutanLista = new List<GameObject>();
    private List<GameObject> gorilaLista = new List<GameObject>();
    
    private Vector3 boundaryPosition1, boundaryPosition2, boundaryPosition3, boundaryPosition4 = default;
    private float timeToSpawn = 5f;
    private float lastSpawnTime = 0f;
    private int maxNumberOfEnemies = 2;
    private int pericoContador, tucanContador, polloContador, pitonContador, capybaraContador, panteraContador, tigreContador, changoContador, orangutanContador, gorilaContador = 0;
    private bool firstSetDefeated, secondSetDefeated, thirdSetDefeated = false;
    private int defeatedEnemies = 0;
    
    public GameObject WinCanvas;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        PopulateBoundaries();
        pericoContador = 0;
        tucanContador = 0;
        polloContador = 0;
        pitonContador = 0;
        capybaraContador = 0;
        panteraContador = 0; 
        tigreContador = 0;
        changoContador = 0;
        orangutanContador = 0;
        gorilaContador = 0;
        maxNumberOfEnemies = 2;
        firstSetDefeated = false;
        secondSetDefeated = false;
        thirdSetDefeated = false;
    }

    private void Update()
    {
        TimerToSpawnEnemyAccordingToFlag();
    }

    private void CreateEnemyAccordingToSpecifiedIndex(int enemyIndex)
    {
        if (!CheckIfItsPossibleToActivateEnemy(enemyIndex))
        {
            return;
        }

        IncreaseEnemyCounter(enemyIndex);

        Vector3 positionToBeSpawned = CalculatePositionToSpawnEnemy();
        
        switch (enemyIndex)
        {
            case 0:
                for (int i = 0; i < pericoLista.Count; i++)
                {
                    if (!pericoLista[i].activeInHierarchy)
                    {
                        pericoLista[i].transform.position = positionToBeSpawned;
                        pericoLista[i].GetComponent<EnemyMovementNavMesh>().TeleportAgentToSpot(positionToBeSpawned);
                        pericoLista[i].GetComponent<EnemyMovementNavMesh>().ResetEnemyStatus();
                        pericoLista[i].SetActive(true);
                        return;
                    }
                }
                break;
            case 1:
                for (int i = 0; i < tucanLista.Count; i++)
                {
                    if (!tucanLista[i].activeInHierarchy)
                    {
                        tucanLista[i].transform.position = positionToBeSpawned;
                        tucanLista[i].GetComponent<EnemyMovementNavMesh>().TeleportAgentToSpot(positionToBeSpawned);
                        tucanLista[i].GetComponent<EnemyMovementNavMesh>().ResetEnemyStatus();
                        tucanLista[i].SetActive(true);
                        return;
                    }
                }
                break;
            case 2:
                for (int i = 0; i < polloLista.Count; i++)
                {
                    if (!polloLista[i].activeInHierarchy)
                    {
                        polloLista[i].transform.position = positionToBeSpawned;
                        polloLista[i].GetComponent<EnemyMovementNavMesh>().TeleportAgentToSpot(positionToBeSpawned);
                        polloLista[i].GetComponent<EnemyMovementNavMesh>().ResetEnemyStatus();
                        polloLista[i].SetActive(true);
                        return;
                    }
                }
                break;
            case 3:
                for (int i = 0; i < pitonLista.Count; i++)
                {
                    if (!pitonLista[i].activeInHierarchy)
                    {
                        pitonLista[i].transform.position = positionToBeSpawned;
                        pitonLista[i].GetComponent<EnemyMovementNavMesh>().TeleportAgentToSpot(positionToBeSpawned);
                        pitonLista[i].GetComponent<EnemyMovementNavMesh>().ResetEnemyStatus();
                        pitonLista[i].SetActive(true);
                        return;
                    }
                }
                break;
            case 4:
                for (int i = 0; i < capybaraLista.Count; i++)
                {
                    if (!capybaraLista[i].activeInHierarchy)
                    {
                        capybaraLista[i].transform.position = positionToBeSpawned;
                        capybaraLista[i].GetComponent<EnemyMovementNavMesh>().TeleportAgentToSpot(positionToBeSpawned);
                        capybaraLista[i].GetComponent<EnemyMovementNavMesh>().ResetEnemyStatus();
                        capybaraLista[i].SetActive(true);
                        return;
                    }
                }
                break;
            case 5:
                for (int i = 0; i < panteraLista.Count; i++)
                {
                    if (!panteraLista[i].activeInHierarchy)
                    {
                        panteraLista[i].transform.position = positionToBeSpawned;
                        panteraLista[i].GetComponent<EnemyMovementNavMesh>().TeleportAgentToSpot(positionToBeSpawned);
                        panteraLista[i].GetComponent<EnemyMovementNavMesh>().ResetEnemyStatus();
                        panteraLista[i].SetActive(true);
                        return;
                    }
                }
                break;
            case 6:
                for (int i = 0; i < tigreLista.Count; i++)
                {
                    if (!tigreLista[i].activeInHierarchy)
                    {
                        tigreLista[i].transform.position = positionToBeSpawned;
                        tigreLista[i].GetComponent<EnemyMovementNavMesh>().TeleportAgentToSpot(positionToBeSpawned);
                        tigreLista[i].GetComponent<EnemyMovementNavMesh>().ResetEnemyStatus();
                        tigreLista[i].SetActive(true);
                        return;
                    }
                }
                break;
            case 7:
                for (int i = 0; i < changoLista.Count; i++)
                {
                    if (!changoLista[i].activeInHierarchy)
                    {
                        changoLista[i].transform.position = positionToBeSpawned;
                        changoLista[i].GetComponent<EnemyMovementNavMesh>().TeleportAgentToSpot(positionToBeSpawned);
                        changoLista[i].GetComponent<EnemyMovementNavMesh>().ResetEnemyStatus();
                        changoLista[i].SetActive(true);
                        return;
                    }
                }
                break;
            case 8:
                for (int i = 0; i < orangutanLista.Count; i++)
                {
                    if (!orangutanLista[i].activeInHierarchy)
                    {
                        orangutanLista[i].transform.position = positionToBeSpawned;
                        orangutanLista[i].GetComponent<EnemyMovementNavMesh>().TeleportAgentToSpot(positionToBeSpawned);
                        orangutanLista[i].GetComponent<EnemyMovementNavMesh>().ResetEnemyStatus();
                        orangutanLista[i].SetActive(true);
                        return;
                    }
                }
                break;
            case 9:
                for (int i = 0; i < gorilaLista.Count; i++)
                {
                    if (!gorilaLista[i].activeInHierarchy)
                    {
                        gorilaLista[i].transform.position = positionToBeSpawned;
                        gorilaLista[i].GetComponent<EnemyMovementNavMesh>().TeleportAgentToSpot(positionToBeSpawned);
                        gorilaLista[i].GetComponent<EnemyMovementNavMesh>().ResetEnemyStatus();
                        gorilaLista[i].SetActive(true);
                        return;
                    }
                }
                break;
        }

        //Si no hay game objects disponibles, entonces instanciamos uno y lo agregamos al pool
        switch (enemyIndex)
        {
            case 0:
                pericoLista.Add(Instantiate(SelectSpecifiedEnemy(enemyIndex), positionToBeSpawned, Quaternion.LookRotation(Vector3.forward, Vector3.up)));
                break;
            case 1:
                tucanLista.Add(Instantiate(SelectSpecifiedEnemy(enemyIndex), positionToBeSpawned, Quaternion.LookRotation(Vector3.forward, Vector3.up)));
                break;
            case 2:
                polloLista.Add(Instantiate(SelectSpecifiedEnemy(enemyIndex), positionToBeSpawned, Quaternion.LookRotation(Vector3.forward, Vector3.up)));
                break;
            case 3:
                pitonLista.Add(Instantiate(SelectSpecifiedEnemy(enemyIndex), positionToBeSpawned, Quaternion.LookRotation(Vector3.forward, Vector3.up)));
                break;
            case 4:
                capybaraLista.Add(Instantiate(SelectSpecifiedEnemy(enemyIndex), positionToBeSpawned, Quaternion.LookRotation(Vector3.forward, Vector3.up)));
                break;
            case 5:
                panteraLista.Add(Instantiate(SelectSpecifiedEnemy(enemyIndex), positionToBeSpawned, Quaternion.LookRotation(Vector3.forward, Vector3.up)));
                break;
            case 6:
                tigreLista.Add(Instantiate(SelectSpecifiedEnemy(enemyIndex), positionToBeSpawned, Quaternion.LookRotation(Vector3.forward, Vector3.up)));
                break;
            case 7:
                changoLista.Add(Instantiate(SelectSpecifiedEnemy(enemyIndex), positionToBeSpawned, Quaternion.LookRotation(Vector3.forward, Vector3.up)));
                break;
            case 8:
                orangutanLista.Add(Instantiate(SelectSpecifiedEnemy(enemyIndex), positionToBeSpawned, Quaternion.LookRotation(Vector3.forward, Vector3.up)));
                break;
            case 9:
                gorilaLista.Add(Instantiate(SelectSpecifiedEnemy(enemyIndex), positionToBeSpawned, Quaternion.LookRotation(Vector3.forward, Vector3.up)));
                break;
        }
    }

    private void PopulateBoundaries()
    {
        boundaryPosition1 = pared1.transform.position;
        boundaryPosition2 = pared2.transform.position;
        boundaryPosition3 = pared3.transform.position;
        boundaryPosition4 = pared4.transform.position;
    }

    private GameObject SelectSpecifiedEnemy(int enemyIndex)
    {
        GameObject newEnemyCreated = default;
        switch (enemyIndex)
        {
            case 0:
                newEnemyCreated = perico;
                break;
            case 1:
                newEnemyCreated = tucan; 
                break;
            case 2:
                newEnemyCreated = pollo;
            break;
            case 3:
                newEnemyCreated = piton;
            break;
            case 4:
                newEnemyCreated = capybara;
            break;
            case 5:
                newEnemyCreated = pantera;
            break;
            case 6:
                newEnemyCreated = tigre;
            break;
            case 7:
                newEnemyCreated = chango;
            break;
            case 8:
                newEnemyCreated = orangutan;
            break;
            case 9:
                newEnemyCreated = gorila;
            break;
        }
        return newEnemyCreated;
    }

    private Vector3 CalculatePositionToSpawnEnemy()
    {
        Vector3 playerPosition = jugador.transform.position;

        // Lista de posibles esquinas donde spawnear (dentro de los límites)
        List<Vector3> spawnPoints = new List<Vector3>
        {
            boundaryPosition1,
            boundaryPosition2,
            boundaryPosition3,
            boundaryPosition4
        };

        // Encuentra la esquina más alejada del jugador
        Vector3 furthestPoint = spawnPoints[0];
        float maxDistance = Vector3.Distance(playerPosition, spawnPoints[0]);

        foreach (Vector3 point in spawnPoints)
        {
            float distance = Vector3.Distance(playerPosition, point);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthestPoint = point;
            }
        }

        // Aplicar un pequeño offset para que no spawnee exactamente en la esquina
        Vector3 directionAwayFromPlayer = (furthestPoint - playerPosition).normalized;
        Vector3 spawnPosition = furthestPoint + directionAwayFromPlayer * 2.5f;

        // Asegura que el Y esté en 0
        spawnPosition.y = 0f;

        return spawnPosition;
    }

    private void TimerToSpawnEnemyAccordingToFlag()
    {
        if (Time.time >= lastSpawnTime + timeToSpawn)
        {
            switch (currentLevel)
            {
                case 1:
                    CreateEnemyAccordingToSpecifiedIndex(0);
                    CreateEnemyAccordingToSpecifiedIndex(1);
                    CreateEnemyAccordingToSpecifiedIndex(2);
                    CreateEnemyAccordingToSpecifiedIndex(3);
                    break;
                case 2:
                    CreateEnemyAccordingToSpecifiedIndex(4);
                    CreateEnemyAccordingToSpecifiedIndex(4);
                    CreateEnemyAccordingToSpecifiedIndex(5);
                    CreateEnemyAccordingToSpecifiedIndex(5);
                    CreateEnemyAccordingToSpecifiedIndex(6);
                    CreateEnemyAccordingToSpecifiedIndex(6);
                    CreateEnemyAccordingToSpecifiedIndex(6);
                    break;
                case 3:
                    CreateEnemyAccordingToSpecifiedIndex(7);
                    CreateEnemyAccordingToSpecifiedIndex(7);
                    CreateEnemyAccordingToSpecifiedIndex(7);
                    CreateEnemyAccordingToSpecifiedIndex(8);
                    CreateEnemyAccordingToSpecifiedIndex(8);
                    CreateEnemyAccordingToSpecifiedIndex(8);
                    CreateEnemyAccordingToSpecifiedIndex(8);
                    CreateEnemyAccordingToSpecifiedIndex(9);
                    CreateEnemyAccordingToSpecifiedIndex(9);
                    CreateEnemyAccordingToSpecifiedIndex(9);
                    break;
            }
            
            lastSpawnTime = Time.time;
        }
    }

    private bool CheckIfItsPossibleToActivateEnemy(int enemyIndexSent)
    {
        bool isEnemyReadyToBeSpawned = false;
        int counterSpecified = 0;
        switch (enemyIndexSent)
        {
            case 0:
                counterSpecified = pericoContador;
                break;
            case 1:
                counterSpecified = tucanContador;
                break;
            case 2:
                counterSpecified = polloContador;
                break;
            case 3:
                counterSpecified = pitonContador;
                break;
            case 4:
                counterSpecified = capybaraContador;
                break;
            case 5:
                counterSpecified = panteraContador;
                break;
            case 6:
                counterSpecified = tigreContador;
                break;
            case 7:
                counterSpecified = changoContador;
                break;
            case 8:
                counterSpecified = orangutanContador;
                break;
            case 9:
                counterSpecified = gorilaContador;
                break;
        }
        if (counterSpecified >= maxNumberOfEnemies)
        {
            isEnemyReadyToBeSpawned = false;
        }
        else
        {
            isEnemyReadyToBeSpawned = true;
        }
        return isEnemyReadyToBeSpawned;
    }

    private void IncreaseEnemyCounter(int enemyIndexSent)
    {
        switch (enemyIndexSent)
        {
            case 0:
                pericoContador++;
                break;
            case 1:
                tucanContador++;
                break;
            case 2:
                polloContador++;
                break;
            case 3:
                pitonContador++;
                break;
            case 4:
                capybaraContador++;
                break;
            case 5:
                panteraContador++;
                break;
            case 6:
                tigreContador++;
                break;
            case 7:
                changoContador++;
                break;
            case 8:
                orangutanContador++;
                break;
            case 9:
                gorilaContador++;
                break;
        }
    }

    public void UpdateEnemyDefeated()
    {
        defeatedEnemies++;
        Debug.Log("Defeated enemies: " + defeatedEnemies);
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (defeatedEnemies >= (pericoContador + tucanContador + polloContador + pitonContador))
        {
            firstSetDefeated = true;
        }
        if (defeatedEnemies >= (capybaraContador + panteraContador + tigreContador))
        {
            secondSetDefeated = true;
        }
        if (defeatedEnemies >= (changoContador + orangutanContador + gorilaContador))
        {
            thirdSetDefeated = true;
        }

        switch (currentLevel)
        {
            case 1:
                if (firstSetDefeated)
                {
                    //CONDICION DE VICTORIA EN EL NIVEL 1
                    WinCanvas.SetActive(true);
                }
                break;
            case 2:
                if (firstSetDefeated && secondSetDefeated)
                {
                    //CONDICION DE VICTORIA EN EL NIVEL 2
                    WinCanvas.SetActive(true);
                }
                break;
            case 3:
                if (firstSetDefeated && secondSetDefeated && thirdSetDefeated)
                {
                    //CONDICION DE VICTORIA EN EL NIVEL 3
                    WinCanvas.SetActive(true);
                }
                break;
        }
    }
}
