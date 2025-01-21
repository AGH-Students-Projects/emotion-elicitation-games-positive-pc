using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject Items;
    public GameObject Healthbar;
    public GameObject TopRightButtons;

    public GameObject PauseUI;
    public GameObject BuyLiveUI;
    public GameObject MainMenuUI;
    public GameObject GameOverUI;
    public GameObject CreditsUI;
    public GameObject InstructionsUI;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowMainMenu()
    {
        MainMenuUI.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        PauseUI.SetActive(true);
    }

    public void ShowGameOverMenu()
    {
        HideAllMenus();
        GameOverUI.SetActive(true);
    }

    public void ShowBuyLiveMenu()
    {
        BuyLiveUI.SetActive(true);
    }

    public void HideAllMenus()
    {
        PauseUI?.SetActive(false);
        MainMenuUI?.SetActive(false);
        GameOverUI?.SetActive(false);
        InstructionsUI?.SetActive(false);
    }

    #region UI Logic
    // ================== Screen Right Menu ==================
    public void OnGamePausePress()
    {
        Time.timeScale = 0;
        PauseUI.SetActive(true);
    }

    // ================== PauseUI ==================
    public void OnGameResumePress()
    {
        Time.timeScale = 1;
        PauseUI.SetActive(false);
        SoundManager.Instance.PauseMusic();
    }

    public void OnGameMainMenuPress()
    {
        GameManager.Instance.ReturnToMainMenu();
    }

    // ================== MainMenuUI ==================
    public void OnGameExitPress()
    {
        Application.Quit();
    }

    public void OnStartPress()
    {
        GameManager.Instance.StartGame();
    }

    public void OnInstructionsPress()
    {
        MainMenuUI.SetActive(false);
        InstructionsUI.SetActive(true);
    }

    public void OnCreditsPress()
    {
        MainMenuUI.SetActive(false);
        CreditsUI.SetActive(true);
    }

    // ================== EndingUI ==================

    public void OnGameRestartPress()
    {
        GameManager.Instance.StartGame();
    }

    // ================== BuyLiveUI ==================
    public void BuyLiveWithDiamonds()
    {
        if (PlayerResources.Instance.Diamonds >= PlayerHealth.Instance.LiveDiamondPrice)
        {
            PlayerResources.Instance.SetDiamonds(PlayerResources.Instance.Diamonds - PlayerHealth.Instance.LiveDiamondPrice);
            PlayerHealth.Instance.AddLife();
            PlayerHealth.Instance.Respawn();
            BuyLiveUI.SetActive(false);
        }
    }

    public void BuyLiveWithCoins()
    {
        if (PlayerResources.Instance.Coins >= PlayerHealth.Instance.LiveCoinPrice)
        {
            PlayerResources.Instance.SetCoins(PlayerResources.Instance.Coins - PlayerHealth.Instance.LiveCoinPrice);
            PlayerHealth.Instance.AddLife();
            PlayerHealth.Instance.Respawn();
            BuyLiveUI.SetActive(false);
        }
    }

    public void OnBuyLiveUIExitPress()
    {
        BuyLiveUI.SetActive(false);
        OnGameMainMenuPress();
    }

    // ================== CreditsUI ==================
    public void OnCreditsPageBackPress()
    {
        CreditsUI.SetActive(false);
        MainMenuUI.SetActive(true);
    }

    // ================== InstructionsUI ==================
    public void OnInstructionsPageBackPress()
    {
        InstructionsUI.SetActive(false);
        MainMenuUI.SetActive(true);
    }

    #endregion
}
