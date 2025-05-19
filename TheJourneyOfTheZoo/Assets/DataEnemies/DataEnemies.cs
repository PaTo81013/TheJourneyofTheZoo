using UnityEngine;

namespace DataEnemies
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies/Enemy Data")]
    public class DataEnemies : ScriptableObject
    {
        public GameObject enemyPrefab;
        public string nombre;
        
        public int vida;
        public int movimiento;
        public int danoMelee;
        public float cooldownMelee;
        
        public int  danoHabilidad;
        public float cooldownHabilidad;
        
        public int puntos;
        public int bonusCritico;
        
        public string levelScene;
    }
}
