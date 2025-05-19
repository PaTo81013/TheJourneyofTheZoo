using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 
    public DataEnemies.DataEnemies dataEnemies;
    
    void Start()
    {
        UpdateScore(0);
    }
    
    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {dataEnemies.puntos + dataEnemies.bonusCritico}";
    }

    public void Points(int score)
    {
        dataEnemies.puntos += score;
    }
    public void BonusPoints(int score)
    {
        dataEnemies.bonusCritico += score;
    }
    
}
