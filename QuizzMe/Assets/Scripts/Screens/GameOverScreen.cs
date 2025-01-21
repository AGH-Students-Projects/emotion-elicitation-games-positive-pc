using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [Header("Buttons")]
    public Button _playAgain;
    public Button _exit;

    [Header("Text")]
    public TextMeshProUGUI _header;
    public TextMeshProUGUI _finalScore;

    [SerializeField] private ColorScheme ColorScheme;

    public void Awake()
    {
        _playAgain.onClick.RemoveAllListeners();
        _playAgain.onClick.AddListener(OnPlayAgainPressed);
        _playAgain.image.color = ColorScheme._primary;
        _playAgain.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._text;

        _exit.onClick.RemoveAllListeners();
        _exit.onClick.AddListener(OnExitPressed);
        _exit.image.color = ColorScheme._danger;
        _exit.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._text;
    }

    private void OnPlayAgainPressed()
    {
        GameManager.Instance.OnBackToStartScreen(_reset: true);
    }

    private void OnExitPressed()
    {
        Application.Quit();
    }

    public void ShowScreen()
    {
        var score = GameManager.Instance.GetScore();

        _header.text = "Ready for another round of surprises?";
        _finalScore.text = $"Your final score: {score}";

        AudioManager.Instance.PlayAudio(AudioManager.AudioType.GameOver);
    }
}
