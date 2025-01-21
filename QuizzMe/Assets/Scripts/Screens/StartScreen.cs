using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button _instructionsButton;

    [SerializeField] private ColorScheme ColorScheme;

    private TouchScreenKeyboard _keyboard;

    public void Initialize()
    {
        _instructionsButton.onClick.RemoveAllListeners();
        _instructionsButton.onClick.AddListener(() => OnInstructionsButtonPressed());

        _instructionsButton.image.color = ColorScheme._primary;
        _instructionsButton.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._text;
    }

    private void OnInstructionsButtonPressed()
    {
        AudioManager.Instance.PlayAudio(AudioManager.AudioType.ButtonClick);
        GameManager.Instance.OnInstructionsButtonPressed();
    }
}
