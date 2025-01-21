using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public int Coins { get; private set; }
    public int Keys { get; private set; }
    public int Diamonds { get; private set; }

    public static PlayerResources Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
    }

    public void SetCoins(int _amount)
    {
        Coins = _amount;
        HealthbarItems.Instance.SetCoinsCountUI(Coins);
    }

    public void SetKeys(int _amount)
    {
        Keys = _amount;
        HealthbarItems.Instance.SetKeyCountUI(Keys);
    }

    public void SetDiamonds(int _amount)
    {
        Diamonds = _amount;
        HealthbarItems.Instance.SetDiamondCountUI(Diamonds);
    }

    public void ResetResources()
    {
        Coins = 0;
        Keys = 0;
        Diamonds = 0;

        HealthbarItems.Instance.SetCoinsCountUI(Coins);
        HealthbarItems.Instance.SetKeyCountUI(Keys);
        HealthbarItems.Instance.SetDiamondCountUI(Diamonds);
    }
}
