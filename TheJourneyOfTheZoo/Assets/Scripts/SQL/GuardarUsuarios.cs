using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class GuardarUsuario : MonoBehaviour
{
    public TMP_InputField inputField; // Arrástralo desde el inspector
    public string url = "https://pato81013.com/TJOTZ/registrar_usuario.php";

    public void GuardarYEnviar()
    {
        string nombre = inputField.text.Trim();

        if (!string.IsNullOrEmpty(nombre))
        {
            PlayerPrefs.SetString("Usuario", nombre);
            Debug.Log("✅ Usuario guardado localmente: " + nombre);
            StartCoroutine(EnviarNombreAlServidor(nombre));
        }
        else
        {
            Debug.LogWarning("⚠️ Campo vacío.");
        }
    }

    IEnumerator EnviarNombreAlServidor(string nombre)
    {
        WWWForm form = new WWWForm();
        form.AddField("nombre", nombre);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Error al enviar al servidor: " + www.error);
        }
        else
        {
            string respuesta = www.downloadHandler.text;
            Debug.Log("🧠 Servidor respondió: " + respuesta);

            if (respuesta == "registro_exitoso")
            {
                Debug.Log("🟢 Usuario registrado en base de datos");
            }
            else if (respuesta == "usuario_existente")
            {
                Debug.Log("🟡 El usuario ya existe");
            }
            else
            {
                Debug.LogWarning("Respuesta inesperada: " + respuesta);
            }
        }
    }
}