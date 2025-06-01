using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; set; }
        public TextMeshProUGUI scoreText; 
        public int totalScore;
        
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
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
                UpdateScore();
            }
        }
    
        void Start()
        {
            UpdateScore();
        }
    
        private void UpdateScore()
        {
            scoreText.text = $"Score {totalScore}";
        }

        private void AddScore(int amount)
        {
            totalScore += amount;
            UpdateScore();
        }

    }
}