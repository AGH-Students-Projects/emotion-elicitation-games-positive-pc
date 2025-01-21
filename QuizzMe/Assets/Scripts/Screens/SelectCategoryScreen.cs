using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectCategoryScreen : MonoBehaviour
{
    [Header("Category Buttons")]
    [SerializeField] private Button _GeneralKnowledge;
    [SerializeField] private Button _Film;
    [SerializeField] private Button _Music;
    [SerializeField] private Button _History;
    [SerializeField] private Button _Sports;
    [SerializeField] private Button _Geography;

    [Header("Difficulty Buttons")]
    [SerializeField] private Button _Easy;
    [SerializeField] private Button _Medium;
    [SerializeField] private Button _Hard;

    [Header("Button Start")]
    [SerializeField] private Button _StartButton;

    [Header("Button Back")]
    [SerializeField] private Button _BackButton;

    private TriviaAPIManager.Category? _SelectedCategory;
    private TriviaAPIManager.Difficulty? _SelectedDifficulty;

    private Button _lastSelectedCategoryButton;
    private Button _lastSelectedDifficultyButton;

    [SerializeField] private ColorScheme ColorScheme;
    public void Initialize()
    {
        // === Category Button Listeners ===
        _GeneralKnowledge.onClick.AddListener(() => SelectCategory(TriviaAPIManager.Category.general_knowledge, _GeneralKnowledge));
        _Film.onClick.AddListener(() => SelectCategory(TriviaAPIManager.Category.film_and_tv, _Film));
        _Music.onClick.AddListener(() => SelectCategory(TriviaAPIManager.Category.music, _Music));
        _History.onClick.AddListener(() => SelectCategory(TriviaAPIManager.Category.history, _History));
        _Sports.onClick.AddListener(() => SelectCategory(TriviaAPIManager.Category.sport_and_leisure, _Sports));
        _Geography.onClick.AddListener(() => SelectCategory(TriviaAPIManager.Category.geography, _Geography));

        // === Difficulty Button Listeners ===
        _Easy.onClick.AddListener(() => SelectDifficulty(TriviaAPIManager.Difficulty.easy, _Easy));
        _Medium.onClick.AddListener(() => SelectDifficulty(TriviaAPIManager.Difficulty.medium, _Medium));
        _Hard.onClick.AddListener(() => SelectDifficulty(TriviaAPIManager.Difficulty.hard, _Hard));

        // === Start Button Listener ===
        _StartButton.onClick.AddListener(OnStartButtonClicked);
        _StartButton.image.color = ColorScheme._primary;
        _StartButton.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._text;

        // === Back Button Listener ===
        _BackButton.onClick.AddListener(OnBackButtonClicked);

        // === Button Colors ===
        _GeneralKnowledge.image.color = ColorScheme._text;
        _GeneralKnowledge.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._accent;

        _Film.image.color = ColorScheme._text;
        _Film.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._accent;

        _Music.image.color = ColorScheme._text;
        _Music.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._accent;

        _History.image.color = ColorScheme._text;
        _History.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._accent;

        _Sports.image.color = ColorScheme._text;
        _Sports.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._accent;

        _Geography.image.color = ColorScheme._text;
        _Geography.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._accent;

        _Easy.image.color = ColorScheme._text;
        _Easy.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._accent;

        _Medium.image.color = ColorScheme._text;
        _Medium.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._accent;

        _Hard.image.color = ColorScheme._text;
        _Hard.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._accent;
    }

    private void SelectCategory(TriviaAPIManager.Category _category, Button _categoryButton)
    {
        _SelectedCategory = _category;

        AudioManager.Instance.PlayAudio(AudioManager.AudioType.ButtonClick);

        ChangeButtonColor(ref _lastSelectedCategoryButton, _categoryButton);
    }

    private void SelectDifficulty(TriviaAPIManager.Difficulty _difficulty, Button _difficultyButton)
    {
        _SelectedDifficulty = _difficulty;

        AudioManager.Instance.PlayAudio(AudioManager.AudioType.ButtonClick);

        ChangeButtonColor(ref _lastSelectedDifficultyButton, _difficultyButton);
    }

    private void OnStartButtonClicked()
    {
        if (_SelectedCategory == null || _SelectedDifficulty == null)
        {
            return;
        }

        AudioManager.Instance.PlayAudio(AudioManager.AudioType.ButtonClick);

        GameManager.Instance.OnCategorySelected(_SelectedCategory.Value);
        GameManager.Instance.OnDifficultySelected(_SelectedDifficulty.Value);
        GameManager.Instance.OnPlayButtonPressed();
    }

    private void OnBackButtonClicked()
    {
        AudioManager.Instance.PlayAudio(AudioManager.AudioType.ButtonClick);
        GameManager.Instance.OnBackToStartScreen(_reset: false);
    }

    private void ChangeButtonColor(ref Button _lastSelectedButton, Button _newlySelectedButton)
    {
        if (_lastSelectedButton != null)
        {
            _lastSelectedButton.image.color = ColorScheme._text;
            _lastSelectedButton.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._accent;
        }

        _newlySelectedButton.image.color = ColorScheme._primary;
        _newlySelectedButton.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._text;

        _lastSelectedButton = _newlySelectedButton;
    }

    public void ResetSelections()
    {
        if (_lastSelectedCategoryButton != null)
        {
            _lastSelectedCategoryButton.image.color = ColorScheme._text;
            _lastSelectedCategoryButton.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._accent;
            _lastSelectedCategoryButton = null;
        }

        if (_lastSelectedDifficultyButton != null)
        {
            _lastSelectedDifficultyButton.image.color = ColorScheme._text;
            _lastSelectedDifficultyButton.GetComponentInChildren<TextMeshProUGUI>().color = ColorScheme._accent;
            _lastSelectedDifficultyButton = null;
        }
    }
}
