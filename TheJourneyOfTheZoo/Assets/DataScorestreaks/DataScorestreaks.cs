using UnityEngine;

namespace DataScorestreaks
{
    [CreateAssetMenu(fileName = "Scorestreaks", menuName = "Scorestreaks/Streak Data")]
    public class DataScorestreaks : ScriptableObject
    {
        [SerializeField] private GameObject ScorestreaksGameObject;
        [SerializeField] private string nombre;
        [SerializeField] private float duracion;
        
        [SerializeField] private int regeneracionDeVida;
        [SerializeField] private int regeneracionDeEscudo;
        
        [SerializeField] private int puntosPorUso;
        [SerializeField] private int cantidadRequerida;
        
    }
}