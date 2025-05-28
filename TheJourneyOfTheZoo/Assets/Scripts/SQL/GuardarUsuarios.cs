using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace SQL
{
    public class SaveSendUser : MonoBehaviour
    {
        public TMP_InputField inputField;
        public string url = "https://pato81013.com/TJOTZ/registrar_usuario.php";

        public void SaveUser()
        {
            string nombre = inputField.text.Trim();

            if (!string.IsNullOrEmpty(nombre))
            {
                PlayerPrefs.SetString("Usuario", nombre);
                Debug.Log("✅ Usuario guardado localmente: " + nombre);
                StartCoroutine(SendNameToServer(nombre));
            }
            else
            {
                Debug.LogWarning("⚠️ Campo vacío.");
            }
        }

        IEnumerator SendNameToServer(string nombre)
        {
            WWWForm form = new WWWForm();
            form.AddField("Nombre",nombre);

            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("❌ Error al enviar al servidor: " + www.error);
            }
            else
            {
                string answer = www.downloadHandler.text;
                Debug.Log("🧠 Servidor respondió: " + answer);

                if (answer == "registro_exitoso")
                {
                    Debug.Log("🟢 Usuario registrado en base de datos");
                }
                else if (answer == "usuario_existente")
                {
                    Debug.Log("🟡 El usuario ya existe");
                }
                else
                {
                    Debug.LogWarning("Respuesta inesperada: " + answer);
                }
            }
        }
    }
}