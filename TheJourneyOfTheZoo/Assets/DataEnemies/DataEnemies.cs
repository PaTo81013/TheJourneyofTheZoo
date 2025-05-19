using UnityEngine;

namespace DataEnemies
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies/Enemy Data")]
    public class DataEnemies : ScriptableObject
    {
        //[SerializeField] private GameObject enemyPrefab;
        [SerializeField] private string nombre;
        
        [SerializeField] public int vida;
        [SerializeField] public int movimiento;
        [SerializeField] public int danoMelee;
        [SerializeField] public float cooldownMelee;
        
        [SerializeField] public int  danoHabilidad;
        [SerializeField] public float cooldownHabilidad;
        
        [SerializeField] public int puntos;
        [SerializeField] public int bonusCritico;
        
        [SerializeField] public string levelScene;
    }
}
