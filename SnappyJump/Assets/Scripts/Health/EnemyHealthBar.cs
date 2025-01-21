using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider HealthbarSlider;
    [SerializeField] private EnemyHealth EnemyHealth;

    void Awake()
    {
        HealthbarSlider.value = EnemyHealth.StartingHealth;
        HealthbarSlider.maxValue = EnemyHealth.StartingHealth;
        HealthbarSlider.minValue = 0;
        HealthbarSlider.wholeNumbers = true;
    }
    
    public void SetHealth(int _health)
    {
        HealthbarSlider.value = _health;
    }

    public void SetMaxHealth(int _health)
    {
        HealthbarSlider.maxValue = _health;
    }
}
