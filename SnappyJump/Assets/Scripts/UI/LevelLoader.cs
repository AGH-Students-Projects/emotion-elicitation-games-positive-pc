using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 2f;

    [SerializeField] private TextMeshProUGUI text;

    public static LevelLoader Instance { get; private set; }

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

    public void LoadLevel(int levelIndex)
    {
        // Set level text
        StartCoroutine(SetText(levelIndex));

        // Load the level
        StartCoroutine(LoadLevelAsync(levelIndex));
    }

    public void RestartCurrentLevel()
    {
        LoadLevel(GameManager.Instance._currentLevelIndex);
    }

    private IEnumerator LoadLevelAsync(int levelIndex)
    {
        // Trigger transition animation
        TriggerFadeOut();

        // Wait for the transition animation to finish
        yield return new WaitForSeconds(transitionTime);

        // Load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Setup player and camera after the scene has loaded
        SetupPlayerAfterLoad();

        // Setup Enemies
        GameObject enemyManager = GameObject.FindWithTag("EnemyManager");

        if (enemyManager != null)
        {
            Debug.Log("Enabling Enemies");
            enemyManager.GetComponent<EnemiesManager>().EnableEnemies();
        }

        // Trigger fade in animation
        TriggerFadeIn();

        yield return new WaitForSeconds(transitionTime);
    }

    private void TriggerFadeOut()
    {
        if (transition != null)
        {
            transition.SetTrigger("StartFadeOut");
        }
    }

    private void TriggerFadeIn()
    {
        if (transition != null)
        {
            transition.SetTrigger("StartFadeIn");
        }
    }

    private void SetupPlayerAfterLoad()
    {
        StartCoroutine(SetupPlayerAndCamera());

        HealthBar.Instance.SetLevel();
    }

    private IEnumerator SetupPlayerAndCamera()
    {
        // Wait one frame to ensure scene objects are initialized
        yield return null;

        GameObject startPoint = GameObject.FindWithTag("Start");

        if (startPoint != null && RespawnManager.Instance != null)
        {
            RespawnManager.Instance.InitializeRespawnPoint();
            RespawnManager.Instance.SetCheckpoint(startPoint.transform.position);

            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                RespawnManager.Instance.PlayerRespawn();
            }
        }

        // Setup the camera for the new level
        CameraControll.Instance.SetupCameraForNewLevel();
    }

    private IEnumerator SetText(int levelIndex)
    {

        var _CurrentLevel = levelIndex + 1;

        text.text = $"Level {_CurrentLevel}";

        yield return null;
    }

    private void HideUI()
    {
        UIManager.Instance.HideAllMenus();
    }

    private void EnableGameplay()
    {
        PlayerMovement.Instance.enabled = true;
        //GetComponent<PlayerAttack>().EnableAttack();
    }

    private void DisableGameplay()
    {
        PlayerMovement.Instance.enabled = false;
        //GetComponent<PlayerAttack>().DisableAttack();
        PlayerHealth.Instance.ResetDeadAnimation();
    }
}
