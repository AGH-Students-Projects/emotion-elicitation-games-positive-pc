using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioClip HurtSound;
    [SerializeField] private AudioClip DieSound;
    [SerializeField] private AudioClip RespawnSound;

    public static PlayerHealth Instance { get; private set; }

    private Animator PlayerAnimator;

    public int StartingHealth { get; private set; } = 3;
    public int Lives { get; private set; } = 3;

    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    public int LiveDiamondPrice { get; private set; } = 2;
    public int LiveCoinPrice { get; private set; } = 5;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }

        CurrentHealth = StartingHealth;
        IsDead = false;

        PlayerAnimator = GetComponent<Animator>();
    }

    public void TakeDamage(float _damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - _damage, 0, StartingHealth);
        HealthBar.Instance.SetHealth((int)CurrentHealth);
        
        if (CurrentHealth > 0)
        {
            PlayerAnimator.SetTrigger("isHurt");
            SoundManager.Instance.PlaySound(HurtSound);
        }
        else
        {
            if (!IsDead)
            {
                IsDead = true;
                RemoveLife();

                // Animation and sound
                PlayerAnimator.SetTrigger("isDead");
                SoundManager.Instance.PlaySound(DieSound);

                // Disable movement
                GetComponent<PlayerMovement>().enabled = false;
                //GetComponent<Rigidbody2D>().gravityScale = 0;
                //GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    public void Heal(int _heal)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + _heal, 0, StartingHealth);
        HealthBar.Instance.SetHealth((int)CurrentHealth);
    }

    public void AddLife()
    {
        Lives++;
        HealthBar.Instance.SetLiveHearts();
    }

    public void RemoveLife()
    {
        Lives--;
        HealthBar.Instance.SetLiveHearts();
    }

    public void Respawn()
    {
        Heal(StartingHealth);
        IsDead = false;

        // Respawn
        RespawnManager.Instance.PlayerRespawn();

        // Animation
        PlayerAnimator.ResetTrigger("isDead");
        PlayerAnimator.Play("Idle");
        SoundManager.Instance.PlaySound(RespawnSound);

        // Movement
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().gravityScale = 1;

        Time.timeScale = 1;
    }

    public void CheckRespawn()
    {
        if (Lives <= 0)
        {
            UIManager.Instance.ShowBuyLiveMenu();
            Time.timeScale = 0;
            return;
        }

        Respawn();
    }

    public void ResetHealth()
    {
        Debug.Log("ResetHealth()");
        IsDead = false;
        CurrentHealth = StartingHealth;
        Lives = 3;

        HealthBar.Instance.SetHealth((int)CurrentHealth);
        HealthBar.Instance.SetLiveHearts();

        Debug.Log($"CurrentHealth: {CurrentHealth}");
        Debug.Log($"Lives: {Lives}");
    }

    public void ResetDeadAnimation()
    {
        PlayerAnimator.ResetTrigger("isDead");
        PlayerAnimator.Play("Idle");
    }
}
