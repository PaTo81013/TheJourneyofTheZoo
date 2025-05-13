using UnityEngine;

namespace DataEnemies
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies/Enemy Data")]
    public class DataEnemies : ScriptableObject
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private string nombre;
        
        [SerializeField] private int vida;
        [SerializeField] private int movimiento;
        [SerializeField] private int danoMelee;
        [SerializeField] private float cooldownMelee;
        
        [SerializeField] private int  danoHabilidad;
        [SerializeField] private float cooldownHabilidad;
        
        [SerializeField] private int puntos;
        [SerializeField] private int bonusCritico;
        
        [SerializeField] private string levelScene;
    }
}
