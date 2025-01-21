using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject[] Hearts;
    [SerializeField] private TextMeshProUGUI Level;

    private Slider HealthbarSlider;

    public static HealthBar Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }

        HealthbarSlider = GetComponent<Slider>();
    }

    void Start()
    {
        HealthbarSlider.minValue = 0;
        HealthbarSlider.maxValue = PlayerHealth.Instance.StartingHealth;
        HealthbarSlider.value = PlayerHealth.Instance.StartingHealth;
        HealthbarSlider.wholeNumbers = true;
    }

    public void SetHealth(int _health)
    {
        HealthbarSlider.value = _health;
    }

    public void SetLiveHearts()
    {
        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i < PlayerHealth.Instance.Lives)
            {
                Hearts[i].SetActive(true);
            }
            else
            {
                Hearts[i].SetActive(false);
            }
        }
    }

    public void SetLevel()
    {
        StartCoroutine(HealthBarLevelSet());
    }

    private IEnumerator HealthBarLevelSet()
    {

        Level.text = $"Level {SceneManager.GetActiveScene().buildIndex + 1}";
        yield return null;
    }
}
