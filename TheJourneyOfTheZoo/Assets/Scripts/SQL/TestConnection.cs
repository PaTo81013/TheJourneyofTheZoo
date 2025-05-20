using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TestConnection : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(CheckConnection());
    }

    IEnumerator CheckConnection()
    {
        string url = "https://pato81013.com/TJOTZ/connect.php";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Respuesta del servidor: " + request.downloadHandler.text);
        }
    }
}