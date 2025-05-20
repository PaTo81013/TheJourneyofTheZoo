using UnityEngine;
using TMPro;

public class ResetUser : MonoBehaviour
{
    public TMP_InputField inputField;

    void Start()
    {
        inputField.text = "";
        PlayerPrefs.DeleteKey("Usuario");
    }
}