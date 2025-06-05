using System;
using TMPro;
using UnityEngine;

public class AmmoUIManager : MonoBehaviour
{
    public static AmmoUIManager Instance { get; set; }
    [SerializeField] private TextMeshProUGUI ammoText = null;
    [SerializeField] private GameObject dashIcon = null;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateNewAmmoValue(45);
    }

    public void UpdateNewAmmoValue(int newAmmoValue)
    {
        ammoText.text = newAmmoValue.ToString() + "/45";
    }

    public void UpdateBananaYagaAmmoValue()
    {
        ammoText.text = "INFINITE";
    }

    public void ToggleDashIcon(bool toggleOption)
    {
        dashIcon.SetActive(toggleOption);
    }
    
}
