using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum ScreenType
    {
        StartScreen,
        InstructionsScreen,
        SelectCategoryScreen,
        QuestionScreen,
        CountdownScreen,
        PauseScreen,
        GameOverScreen
    }

    [Header("UI Screens")]
    public GameObject _startScreen;
    public GameObject _instructionsScreen;
    public GameObject _selectCategoryScreen;
    public GameObject _questionScreen;
    public GameObject _countdownScreen;
    public GameObject _gameOverScreen;

    public ColorScheme ColorScheme;

    public void ShowScreen(ScreenType _ScreenType)
    {
        // Hide all screens
        _startScreen.SetActive(false);
        _instructionsScreen.SetActive(false);
        _selectCategoryScreen.SetActive(false);
        _questionScreen.SetActive(false);
        _countdownScreen.SetActive(false);
        _gameOverScreen.SetActive(false);

        switch (_ScreenType)
        {
            case ScreenType.StartScreen:
                _startScreen.SetActive(true);
                break;

            case ScreenType.InstructionsScreen:
                _instructionsScreen.SetActive(true);
                break;

            case ScreenType.SelectCategoryScreen:
                _selectCategoryScreen.SetActive(true);
                break;

            case ScreenType.QuestionScreen:
                _questionScreen.SetActive(true);
                break;

            case ScreenType.CountdownScreen:
                _countdownScreen.SetActive(true);
                break;

            case ScreenType.GameOverScreen:
                _gameOverScreen.SetActive(true);
                break;

            default:
                break;
        }
    }

    // === Screens === //
    public void SelectCategoryScreen()
    {
        ShowScreen(ScreenType.SelectCategoryScreen);

        SelectCategoryScreen selectCategoryScreen = _selectCategoryScreen.GetComponent<SelectCategoryScreen>();
        selectCategoryScreen.Initialize();
    }

    public void StartScreen()
    {
        ShowScreen(ScreenType.StartScreen);

        if (_startScreen.TryGetComponent<StartScreen>(out var startScreen))
            startScreen.Initialize();
    }

    public void InstructionsScreen()
    {
        ShowScreen(ScreenType.InstructionsScreen);
        InstructionsScreen instructionsScreen = _instructionsScreen.GetComponent<InstructionsScreen>();
        instructionsScreen.Initialize();
    }

    public void HideInstructionsScreen()
    {
        ShowScreen(ScreenType.StartScreen);
        _instructionsScreen.SetActive(false);
    }

    public void QuestionScreen(TriviaAPIManager.QuizzQuestion _question, float _timePerQuestion)
    {
        ShowScreen(ScreenType.QuestionScreen);

        QuestionScreen questionScreen = _questionScreen.GetComponent<QuestionScreen>();
        questionScreen.Initialize();
        questionScreen.DisplayQuestion(_question, _timePerQuestion);
    }

    public QuestionScreen GetActiveQuestionScreen()
    {
        if (_questionScreen.activeSelf)
        {
            return _questionScreen.GetComponent<QuestionScreen>();
        }
        else
        {
            return null;
        }
    }

    public void GameOverScreen()
    {
        ShowScreen(ScreenType.GameOverScreen);

        GameOverScreen gameOverScreen = _gameOverScreen.GetComponent<GameOverScreen>();
        gameOverScreen.ShowScreen();
    }

    public void CountdownScreen(int _questionIndex, System.Action _onCountdownCompleteAction)
    {
        ShowScreen(ScreenType.CountdownScreen);

        CountdownScreen countdownScreen = _countdownScreen.GetComponent<CountdownScreen>();
        countdownScreen.Initialize(_questionIndex, _onCountdownCompleteAction);
        countdownScreen.StartCountdown();
    }

    public void ResetUIElements()
    {
        if (_selectCategoryScreen.TryGetComponent<SelectCategoryScreen>(out var selectCategoryScreen))
            selectCategoryScreen.ResetSelections();
    }
}