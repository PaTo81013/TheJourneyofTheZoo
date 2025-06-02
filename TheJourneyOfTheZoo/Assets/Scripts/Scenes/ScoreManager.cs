using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class ScoreManager : MonoBehaviour
    { 
        [SerializeField] public DataScorestreaks.DataScorestreaks scoreStreak1Data;
        [SerializeField] public DataScorestreaks.DataScorestreaks scoreStreak2Data;
        [SerializeField] public DataScorestreaks.DataScorestreaks scoreStreak3Data;
        [SerializeField] public DataScorestreaks.DataScorestreaks scoreStreak4Data;
        public static ScoreManager Instance { get; set; }
        public TextMeshProUGUI scoreText; 
        private int totalScore, scorestreak1Counter, scorestreak2Counter, scorestreak3Counter, scorestreak4Counter = 0;
        private bool scorestreak1READY, scorestreak2READY, scorestreak3READY, scorestreak4READY = false;
        private bool bananaYaga = false;
        
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
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
        }

        void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "MainMenu")
            {
                totalScore = 0;
                UpdateScoreInUIDisplay();
            }
        }
    
        void Start()
        {
            totalScore = 0;
            InterruptScorestreakCounters();
            UpdateScoreInUIDisplay();
            bananaYaga = false;
        }
    
        private void UpdateScoreInUIDisplay()
        {
            scoreText.text = $"Score {totalScore}";
        }

        public void IncreaseScore(int amount)
        {
            totalScore += amount;
            Debug.Log("increased score by: " + amount);
            scorestreak1Counter += amount;
            scorestreak2Counter += amount;
            scorestreak3Counter += amount;
            scorestreak4Counter += amount;
            CheckScorestreakAvailability();
            UpdateScoreInUIDisplay();
        }

        public void PlayerHasBeenHit()
        {
            
            InterruptScorestreakCounters();
            UpdateScoreInUIDisplay();
        }

        public int GetScore()
        {
            return totalScore;
        }

        private void InterruptScorestreakCounters()
        {
            scorestreak1Counter = 0;
            scorestreak2Counter = 0;
            scorestreak3Counter = 0;
            scorestreak4Counter = 0;
        }

        private void CheckScorestreakAvailability()
        {
            if (scorestreak1Counter >= scoreStreak1Data.cantidadRequerida && !scorestreak1READY)
            {
                scorestreak1READY = true;
            }
            if (scorestreak2Counter >= scoreStreak2Data.cantidadRequerida && !scorestreak2READY)
            {
                scorestreak2READY = true;
            }
            if (scorestreak3Counter >= scoreStreak3Data.cantidadRequerida && !scorestreak3READY)
            {
                scorestreak3READY = true;
            }
            if (scorestreak4Counter >= scoreStreak4Data.cantidadRequerida && !scorestreak4READY)
            {
                scorestreak4READY = true;
            }
        }

        public void ScorestreakActivated(int numberScorestreak)
        {
            switch (numberScorestreak)
            {
                case 1:
                    scorestreak1READY = false;
                    scorestreak1Counter = 0;
                    break;
                case 2:
                    scorestreak2READY = false;
                    scorestreak2Counter = 0;
                    break;
                case 3:
                    scorestreak3READY = false;
                    scorestreak3Counter = 0;
                    break;
                case 4:
                    scorestreak4READY = false;
                    scorestreak4Counter = 0;
                    break;
            }
        }

        public bool GetBananaYagaStatus()
        {
            return bananaYaga;
        }

    }
}