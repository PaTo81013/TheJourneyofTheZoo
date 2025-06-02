using TMPro;

using UnityEngine;

public class HitpointsUIManager : MonoBehaviour
{
    public static HitpointsUIManager Instance { get; set; }
    [SerializeField] private TextMeshProUGUI hitpointsText = null;
    [SerializeField] private TextMeshProUGUI shieldpointsText = null;

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
        UpdateNewHPValue(100);
        UpdateNewShieldValue(0);
    }

    public void UpdateNewHPValue(int newHPValue)
    {
        hitpointsText.text = newHPValue.ToString();
    }

    public void UpdateNewShieldValue(int newShieldValue)
    {
        shieldpointsText.text = newShieldValue.ToString();
    }
}
