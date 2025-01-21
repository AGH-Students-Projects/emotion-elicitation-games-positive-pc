using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource Source;
    public static SoundManager Instance { get; private set; }

    public AudioClip winSound;
    public AudioSource backgroundMusic;

    void Awake()
    {
        Source = GetComponent<AudioSource>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        Source.PlayOneShot(clip);
    }

    public void PauseMusic()
    {
        Source.Stop();
    }

    public void PlayBackgroundMusic()
    {
        backgroundMusic.Play();
    }
}
