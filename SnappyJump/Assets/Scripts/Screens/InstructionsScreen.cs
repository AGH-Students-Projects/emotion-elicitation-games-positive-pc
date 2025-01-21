using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsScreen : MonoBehaviour
{
    [SerializeField] private Button _playButton;

    [SerializeField] private ColorScheme ColorScheme;

    public void Initialize()
    {
        _playButton.onClick.RemoveAllListeners();
        _playButton.onClick.AddListener(OnStartButtonPressed);

        _playButton.image.color = ColorScheme._primary;
        _playButton.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._text;
    }

    private void OnStartButtonPressed()
    {
        AudioManager.Instance.PlayAudio(AudioManager.AudioType.ButtonClick);
        GameManager.Instance.OnStartButtonPressed();
    }
}
