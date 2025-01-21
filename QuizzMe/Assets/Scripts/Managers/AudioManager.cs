using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource _audioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _buttonClick;
    [SerializeField] private AudioClip _correctAnswer;
    [SerializeField] private AudioClip _wrongAnswer;
    [SerializeField] private AudioClip _clockTick;
    [SerializeField] private AudioClip _timeUp;
    [SerializeField] private AudioClip _gameOver;
    [SerializeField] private AudioClip _drumRoll;

    public enum AudioType
    {
        BackgroundMusic,
        ButtonClick,
        CorrectAnswer,
        WrongAnswer,
        ClockTick,
        TimeUp,
        GameOver,
        DrumRoll
    }

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
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.ButtonClick:
                _audioSource.PlayOneShot(_buttonClick);
                break;
            case AudioType.CorrectAnswer:
                _audioSource.PlayOneShot(_correctAnswer);
                break;
            case AudioType.WrongAnswer:
                _audioSource.PlayOneShot(_wrongAnswer);
                break;
            case AudioType.ClockTick:
                _audioSource.PlayOneShot(_clockTick);
                break;
            case AudioType.TimeUp:
                _audioSource.PlayOneShot(_timeUp);
                break;
            case AudioType.GameOver:
                _audioSource.PlayOneShot(_gameOver);
                break;
            case AudioType.DrumRoll:
                _audioSource.PlayOneShot(_drumRoll);
                break;
        }
    }

    #region Audio Utils Management
    public void StopAudio()
    {
        _audioSource.Stop();
    }

    public void PauseAudio()
    {
        _audioSource.Pause();
    }

    public void UnPauseAudio()
    {
        _audioSource.UnPause();
    }

    public void SetVolume(float volume)
    {
        _audioSource.volume = volume;
    }

    #endregion

    public bool IsPlaying()
    {
        return _audioSource.isPlaying;
    }
}
