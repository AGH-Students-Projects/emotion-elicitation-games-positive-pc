using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] public int StartingHealth;
    [SerializeField] private EnemyHealthBar EnemyHealthBar;

    [Header("Sound")]
    [SerializeField] private AudioClip HurtSound;
    [SerializeField] private AudioClip DieSound;

    private Animator EnemyAnimator;
    private BoxCollider2D EnemyBoxCollider2D;

    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }


    void Awake()
    {
        CurrentHealth = StartingHealth;
        IsDead = false;
        EnemyAnimator = GetComponent<Animator>();
        EnemyBoxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void TakeDamage(float _damage)
    {

        CurrentHealth = Mathf.Clamp(CurrentHealth - _damage, 0, StartingHealth);
        EnemyHealthBar.SetHealth((int)CurrentHealth);

        if (CurrentHealth > 0)
        {
            EnemyAnimator.SetTrigger("isHurt");
            SoundManager.Instance.PlaySound(HurtSound);
        }
        else
        {
            if (!IsDead)
            {
                IsDead = true;
                EnemyAnimator.SetTrigger("isDead");
                SoundManager.Instance.PlaySound(DieSound);
                EnemyBoxCollider2D.enabled = false;
                Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            }
        }
    }
}

