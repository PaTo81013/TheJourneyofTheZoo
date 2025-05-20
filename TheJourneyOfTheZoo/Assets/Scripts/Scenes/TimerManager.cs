using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerTxt = default;
    [SerializeField] private float limitTime = 40f;

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

    private void UpdateTimerText(float currentTimer, TextMeshProUGUI cronometer)
    {
        float minutes = Mathf.Floor(currentTimer / 60);
        float seconds = Mathf.Floor(currentTimer % 60);
        cronometer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetTimer()
    {
        _currentTime = 0f; // 🔥 Empieza en 0
        _timerRunning = true;
        UpdateTimerText(_currentTime, timerTxt);
    }
    
}