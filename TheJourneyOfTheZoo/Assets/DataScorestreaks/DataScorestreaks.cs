using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DataScorestreaks
{
    [CreateAssetMenu(fileName = "Scorestreaks", menuName = "Scorestreaks/Streak Data")]
    public class DataScorestreaks : ScriptableObject
    {
        public GameObject Prefab => scorestreaksPrefab;
        
        [SerializeField] private GameObject scorestreaksPrefab;
        public string nombre;
        public float duracion;
        
        public int regeneracionDeVida;
        public int regeneracionDeEscudo;
        
        public int puntosPorUso;
        public int cantidadRequerida;
        public float cooldown;
        
    }
}