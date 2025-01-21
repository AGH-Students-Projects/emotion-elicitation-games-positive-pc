using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionScreen : MonoBehaviour
{
    [Header("UI Question Elements")]
    [SerializeField] private Slider _timerSlider;
    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private List<Button> _answerButtons;

    [Header("UI Buttons")]
    [SerializeField] private Button _submitButton;

    [Header("Gradient")]
    [SerializeField] private Gradient _timerGradient;
    [SerializeField] private Image _sliderFillImage;

    private string _selectedAnswer;
    private bool _isAnswerSubmitted;

    [SerializeField] private ColorScheme ColorScheme;

    public void Initialize()
    {
        foreach (Button button in _answerButtons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnAnswerSelected(button));
        }

        _submitButton.onClick.RemoveAllListeners();
        _submitButton.onClick.AddListener(OnSubmitButtonPressed);
        _submitButton.image.color = ColorScheme._primary;
        _submitButton.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._text;
    }

    public void DisplayQuestion(TriviaAPIManager.QuizzQuestion _question, float _questionTime)
    {
        _questionText.text = _question.Question;

        List<string> allAnswers = new(_question.IncorrectAnswers)
        {
            _question.CorrectAnswer
        };

        allAnswers = Shuffle(allAnswers);

        // Assigning answers to buttons
        for (int i = 0; i < _answerButtons.Count; i++)
        {
            if (i < allAnswers.Count)
            {
                _answerButtons[i].gameObject.SetActive(true);
                _answerButtons[i].interactable = true;
                _answerButtons[i].image.color = ColorScheme._text;
                _answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = allAnswers[i];
            }
            else
            {
                _answerButtons[i].gameObject.SetActive(false);
            }
        }

        _submitButton.interactable = true;

        _selectedAnswer = null;
        _isAnswerSubmitted = false;
        _timerSlider.gameObject.SetActive(true);

        StartTimer(_questionTime);
    }

    private void OnAnswerSelected(Button _button)
    {
        foreach (Button button in _answerButtons)
        {
            button.image.color = ColorScheme._text;
        }

        _button.image.color = ColorScheme._warning;
        _button.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._accent;

        AudioManager.Instance.PlayAudio(AudioManager.AudioType.ButtonClick);

        _selectedAnswer = _button.GetComponentInChildren<TextMeshProUGUI>().text;
    }

    private void OnSubmitButtonPressed()
    {
        if (_isAnswerSubmitted)
        {
            Debug.LogWarning("Answer already submitted!");
            return;
        }

        if (string.IsNullOrEmpty(_selectedAnswer))
        {
            Debug.LogWarning("No answer selected!");
            return;
        }

        _isAnswerSubmitted = true;

        DisableButtons();

        AudioManager.Instance.PlayAudio(AudioManager.AudioType.ButtonClick);

        GameManager.Instance.SubmitAnswer(_selectedAnswer);
    }

    public IEnumerator ShowFeedback()
    {
        _timerSlider.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        string correctAnswer = GameManager.Instance.GetCorrectAnswer();

        foreach (Button button in _answerButtons)
        {
            string answerText = button.GetComponentInChildren<TextMeshProUGUI>().text;

            if (answerText == correctAnswer)
            {
                button.image.color = ColorScheme._accept;
            }
            else if (answerText == _selectedAnswer && _selectedAnswer != correctAnswer)
            {
                button.image.color = ColorScheme._danger;
            }
        }

        // Play audio based on the answer
        if (_selectedAnswer == correctAnswer)
        {
            AudioManager.Instance.PlayAudio(AudioManager.AudioType.CorrectAnswer);
        }
        else
        {
            AudioManager.Instance.PlayAudio(AudioManager.AudioType.WrongAnswer);
        }

        yield return new WaitForSeconds(1f);

        GameManager.Instance.OnFeedbackDisplayed();
    }

    private void StartTimer(float _duration)
    {
        _timerSlider.maxValue = _duration;
        _timerSlider.value = _duration;

        StopAllCoroutines();
        StartCoroutine(TimerCoroutine(_duration));
    }

    public float GetRemainingQuestionTime()
    {
        return _timerSlider.value;
    }

    private IEnumerator TimerCoroutine(float _duration)
    {
        float _timeLeft = _duration; 

        while (_timeLeft > 0 && !_isAnswerSubmitted)
        {
            _timeLeft -= Time.deltaTime;
            _timerSlider.value = _timeLeft;

            float normalizedTime = _timeLeft / _duration;
            _sliderFillImage.color = _timerGradient.Evaluate(normalizedTime);

            yield return null;
        }

        if (!_isAnswerSubmitted)
        {
            DisableButtons();

            if (_selectedAnswer == null)
            {
                GameManager.Instance.SubmitAnswer(null);
            } else
            {
                GameManager.Instance.SubmitAnswer(_selectedAnswer);
            }
        }
    }

    private void DisableButtons()
    {
        foreach (Button button in _answerButtons)
        {
            button.interactable = false;
        }

        _submitButton.interactable = false;
    }
    private List<string> Shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
        }
        return list;
    }

    public void ResetUI()
    {
        _timerSlider.gameObject.SetActive(false);
        _timerSlider.value = _timerSlider.maxValue;

        foreach (Button button in _answerButtons)
        {
            button.interactable = false;
            button.image.color = ColorScheme._text;
            button.gameObject.SetActive(false);
        }

        _submitButton.interactable = false;

        _questionText.text = string.Empty;
    }
}


