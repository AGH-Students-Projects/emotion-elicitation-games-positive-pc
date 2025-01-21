using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }
    public PlayerInputActions InputActions { get; private set; }

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

        InputActions = new PlayerInputActions();
        InputActions.Enable();
    }

    private void OnDestroy()
    {
        InputActions?.Dispose();
    }
}
