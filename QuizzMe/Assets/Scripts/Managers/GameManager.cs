using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        GameStartState,
        GameSelectCategoryState,
        GameCountdownState,
        GameQuestionState,
        GameAnswerRevealState,
        GameOverState
    }

    private GameState _currentState;
    private TriviaAPIManager.QuizzQuestion[] _questions;

    private int _currentQuestionIndex;
    private int _correctAnswers;

    private TriviaAPIManager.Category _selectedCategory;
    private TriviaAPIManager.Difficulty _selectedDifficulty;

    [Header("UI Manager")]
    [SerializeField] private UIManager UIManager;
    public ColorScheme ColorScheme;

    [Header("Gameplay Settings")]
    [SerializeField] private float _timePerQuestion = 15f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ChangeState(GameState.GameStartState);
    }

    private void ChangeState(GameState _newState)
    {
        _currentState = _newState;

        switch (_newState)
        {
            case GameState.GameStartState:
                UIManager.StartScreen();
                break;

            case GameState.GameSelectCategoryState:
                UIManager.SelectCategoryScreen();
                break;

            case GameState.GameCountdownState:
                StartCountdown();
                break;

            case GameState.GameQuestionState:
                StartQuestion();
                break;

            case GameState.GameAnswerRevealState:
                HandleAnswerReveal();
                break;

            case GameState.GameOverState:
                UIManager.GameOverScreen();
                break;
            default:
                break;
        }
    }

    public void OnStartButtonPressed()
    {
        ChangeState(GameState.GameSelectCategoryState);
    }

    public void OnInstructionsButtonPressed()
    {
        UIManager.InstructionsScreen();
    }

    public void OnInstructionsBackButtonPressed()
    {
        UIManager.HideInstructionsScreen();
    }

    public void OnCategorySelected(TriviaAPIManager.Category category)
    {
        _selectedCategory = category;
    }

    public void OnDifficultySelected(TriviaAPIManager.Difficulty difficulty)
    {
        _selectedDifficulty = difficulty;
    }

    public void OnBackToStartScreen(bool _reset)
    {
        if (_reset)
        {
            ResetGameProperties();

            StopAllCoroutines();

            var activeQuestionScreen = UIManager.GetActiveQuestionScreen();

            if (activeQuestionScreen != null)
            {
                activeQuestionScreen.ResetUI();
            }

            UIManager.ResetUIElements();
        }

        ChangeState(GameState.GameStartState);
    }

    public void OnPlayButtonPressed()
    {
        _questions = null;

        // Fetch questions
        TriviaAPIManager.Instance.FetchTriviaQuestions(_selectedCategory, _selectedDifficulty);

        // Start the game
        StartCoroutine(WaitForQuestionsAndStart());
    }

    private IEnumerator WaitForQuestionsAndStart()
    {
        while (TriviaAPIManager.Instance._questionArray == null)
        {
            yield return null;
        }

        _questions = TriviaAPIManager.Instance._questionArray;
        _currentQuestionIndex = 0;

        ChangeState(GameState.GameCountdownState);
    }

    private void StartCountdown()
    {
        //UIManager.CountdownScreen(_currentQuestionIndex, () =>
        //{
        //    ChangeState(GameState.GameQuestionState);
        //});

        // Without the countdown screen between the questions
        ChangeState(GameState.GameQuestionState);
    }

    private void StartQuestion()
    {
        if (_currentQuestionIndex < _questions.Length)
        {
            // Display question
            TriviaAPIManager.QuizzQuestion _currentQuestion = _questions[_currentQuestionIndex];
            UIManager.QuestionScreen(_currentQuestion, _timePerQuestion);
        }
        else
            ChangeState(GameState.GameOverState);
    }

    public void SubmitAnswer(string _answer)
    {
        if (_answer == GetCorrectAnswer())
        {
            _correctAnswers++;
        }

        ChangeState(GameState.GameAnswerRevealState);
    }

    private void HandleAnswerReveal()
    {
        var questionScreen = UIManager.GetActiveQuestionScreen();
        
        if (questionScreen != null)
        {
            StartCoroutine(questionScreen.ShowFeedback());
        }
    }

    public string GetCorrectAnswer()
    {
        return _questions[_currentQuestionIndex].CorrectAnswer;
    }

    public void OnFeedbackDisplayed()
    {
        _currentQuestionIndex++;
        
        if (_currentQuestionIndex < _questions.Length)
        {
            ChangeState(GameState.GameCountdownState);
        }
        else
        {
            ChangeState(GameState.GameOverState);
        }
    }

    public int GetScore()
    {
        return _correctAnswers; 
    }

    private void ResetGameProperties()
    {
        _currentQuestionIndex = 0;
        _correctAnswers = 0;
        _questions = null;
        _selectedCategory = default;
        _selectedDifficulty = default;
    }
}
