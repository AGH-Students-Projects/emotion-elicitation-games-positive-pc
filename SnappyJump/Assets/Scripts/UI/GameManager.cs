using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject mobileControlsPanel;

    private GameState _gameState;
    public enum GameState
    {
        MainMenu,
        Playing,
        Pause,
        GameOver
    }

    public int _currentLevelIndex { get; private set; } = -1;

    void Awake()
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

    private void Start()
    {
        SetGameState(GameState.MainMenu);
    }

    public void SetGameState(GameState state)
    {
        _gameState = state;

        switch (_gameState)
        {
            case GameState.MainMenu:
                Time.timeScale = 0;
                UIManager.Instance.ShowMainMenu();
                break;
            case GameState.Playing:
                Time.timeScale = 1;
                break;
            case GameState.Pause:
                Time.timeScale = 0;
                UIManager.Instance.ShowPauseMenu();
                break;
            case GameState.GameOver:
                UIManager.Instance.ShowGameOverMenu();
                break;
            default:
                break;
        }
    }

    public void StartGame()
    {
        // Start from level 1 (index 0)
        _currentLevelIndex = -1;

        // Play background music
        SoundManager.Instance.PlayBackgroundMusic();
        RespawnManager.Instance.PlayerRespawn();

        // Load the next level
        LoadNextLevel();

        // Reset player stats
        PlayerHealth.Instance.ResetHealth();
        PlayerResources.Instance.ResetResources();

        // Set game state to playing
        SetGameState(GameState.Playing);
    }

    public void ReturnToMainMenu()
    {
        SetGameState(GameState.MainMenu);
    }

    public void PauseGame()
    {
        SetGameState(GameState.Pause);
    }

    public void ResumeGame()
    {
        SetGameState(GameState.Playing);
    }

    public void LoadNextLevel()
    {
        // Increment the level index
        _currentLevelIndex++;

        // Check if it's the last level
        if (_currentLevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Reset player resources for the next level
            PlayerResources.Instance.ResetResources();

            // Load the next level
            LevelLoader.Instance.LoadLevel(_currentLevelIndex);

            return;
        }

        // If it's the last level, show the win screen
        SoundManager.Instance.PlaySound(SoundManager.Instance.winSound);
        
        SetGameState(GameState.GameOver);

        StartCoroutine(AfterWin());
    }

    private IEnumerator AfterWin()
    {
        yield return null;

        Time.timeScale = 1;

        PlayerMovement.Instance.SetAnimator();
        PlayerMovement.Instance.enabled = false;
    }

    private IEnumerator ResetPlayerStats()
    {
        yield return null;
        PlayerHealth.Instance.ResetHealth();
        PlayerResources.Instance.ResetResources();
    }
}
