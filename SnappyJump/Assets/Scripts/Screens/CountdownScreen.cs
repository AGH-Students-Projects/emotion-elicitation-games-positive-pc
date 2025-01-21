using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownScreen : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private TextMeshProUGUI _questionCountText;
    [SerializeField] private Image _backgroundImage;    

    [Header("Countdown Settings")]
    [SerializeField] private float _countdownTime = 3f;

    private System.Action _onCountdownComplete;

    [SerializeField] private ColorScheme ColorScheme;

    public void Initialize(int _questionIndex, System.Action _onCountdownCompleteAction)
    {
        _questionCountText.text = $"Question {_questionIndex + 1}";
        _onCountdownComplete = _onCountdownCompleteAction;

        _countdownText.color = ColorScheme._text;
        _questionCountText.color = ColorScheme._text;
        _backgroundImage.color = ColorScheme._accent;
    }

    public void StartCountdown()
    {
        StartCoroutine(CountdownTimer());
    }

    private IEnumerator CountdownTimer()
    {
        float _countdown = _countdownTime;

        while (_countdown > 0)
        {
            _countdownText.text = Mathf.CeilToInt(_countdown).ToString();
            AudioManager.Instance.PlayAudio(AudioManager.AudioType.ClockTick);

            yield return new WaitForSeconds(1f);

            _countdown--;
        }

        _onCountdownComplete?.Invoke();
    }
}
