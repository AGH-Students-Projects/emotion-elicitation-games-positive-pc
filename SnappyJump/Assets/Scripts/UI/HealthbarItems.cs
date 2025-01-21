using TMPro;
using UnityEngine;

public class HealthbarItems : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI KeyCount;
    [SerializeField] private TextMeshProUGUI DiamondCount;
    [SerializeField] private TextMeshProUGUI CoinsCount;

    public static HealthbarItems Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }

        InitializeValues();
    }

    public void SetKeyCountUI(int _amount)
    {
        KeyCount.text = _amount.ToString();
    }

    public void SetDiamondCountUI(int _amount)
    {
        DiamondCount.text = _amount.ToString();
    }

    public void SetCoinsCountUI(int _amount)
    {
        CoinsCount.text = _amount.ToString();
    }

    public void OnWillRenderObject()
    {
        KeyCount.text = 0.ToString();
        DiamondCount.text = 0.ToString();
        CoinsCount.text = 0.ToString();
    }

    private void InitializeValues()
    {
        KeyCount.text = PlayerResources.Instance.Keys.ToString();
        DiamondCount.text = PlayerResources.Instance.Diamonds.ToString();
        CoinsCount.text = PlayerResources.Instance.Coins.ToString();
    }
}
