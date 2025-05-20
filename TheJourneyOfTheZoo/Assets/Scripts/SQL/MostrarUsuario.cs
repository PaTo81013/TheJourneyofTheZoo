using UnityEngine;
using TMPro;

public class MostrarUsuario : MonoBehaviour
{
    public TMP_Text texto;

    void Start()
    {
        string nombre = PlayerPrefs.GetString("Usuario");
        texto.text =  nombre;
    }
}