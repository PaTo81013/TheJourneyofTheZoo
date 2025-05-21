using TMPro;
using UnityEngine;

namespace SQL
{
    public class ShowUser : MonoBehaviour
    {
        public TMP_Text text;

        void Start()
        {
            string nombre = PlayerPrefs.GetString("Usuario");
            text.text =  nombre;
        }
    }
}