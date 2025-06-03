using TMPro;
using UnityEngine;

namespace Scenes
{
    public class TimerManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerTxt = null;
        [SerializeField] private float limitTime = 10000f;

        private float _currentTime;
        private bool _timerRunning = true;

        private void Start()
        {
            ResetTimer();
        }

        private void Update()
        {
            if (!_timerRunning) return;
            if (Pause.IsPaused) return;

            _currentTime += Time.unscaledDeltaTime;

            if (_currentTime >= limitTime)
            {
                _currentTime = limitTime;
                _timerRunning = false;
            }

            UpdateTimerText(_currentTime, timerTxt);
        }

        private void UpdateTimerText(float currentTimer, TextMeshProUGUI chronometer)
        {
            float minutes = Mathf.Floor(currentTimer / 60);
            float seconds = Mathf.Floor(currentTimer % 60);
            chronometer.text = $"{minutes:00}:{seconds:00}";
        }

        private void ResetTimer()
        {
            _currentTime = 0f;
            _timerRunning = true;
            UpdateTimerText(_currentTime, timerTxt);
        }
    
    }
}