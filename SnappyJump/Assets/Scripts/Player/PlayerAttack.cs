using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float AttackCooldown;
    [SerializeField] private float AttackDamage;
    [SerializeField] private Transform PlayerAttackPoint;

    [Header("Enemies")]
    [SerializeField] private LayerMask EnemyLayer;

    [Header("Sound")]
    [SerializeField] private AudioClip AttackSound;
    [SerializeField] private AudioClip NoEnemySound;

    private float CooldownTimer = Mathf.Infinity;
    [SerializeField] private float AttackRange = 1f;

    private Animator PlayerAnimator;
    private PlayerMovement playerMovement;
    private PlayerInputActions playerInputActions;

    void Awake()
    {
        PlayerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Gameplay.Enable();
        playerInputActions.Gameplay.Attack.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {   
        playerInputActions.Gameplay.Attack.performed -= OnAttackPerformed;
        playerInputActions.Gameplay.Disable();
    }

    void Update()
    {
        CooldownTimer += Time.deltaTime;
    }

    private void OnAttackPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        CheckAttack();
    }

    private void CheckAttack()
    {
        if (CooldownTimer > AttackCooldown && playerMovement.CanAttack())
        {
            PlayerAnimator.SetTrigger("Attack");
            Attack();
        }
    }

    private void Attack()
    {
        CooldownTimer = 0;
        DamageEnemies();
    }

    private void DamageEnemies()
    {
        var enemies = EnemiesInAttackRange();

        if (enemies.Length == 0) { 
            SoundManager.Instance.PlaySound(NoEnemySound);
            return;
        }

        SoundManager.Instance.PlaySound(AttackSound);

        foreach (Collider2D enemy in enemies)
        {
            var enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(AttackDamage);
            }
        }
    }

    private Collider2D[] EnemiesInAttackRange()
    {
        if (PlayerAttackPoint == null)
        {
            Debug.LogWarning("PlayerAttackPoint is not assigned!");
            return new Collider2D[0];
        }

        return Physics2D.OverlapCircleAll(PlayerAttackPoint.position, AttackRange, EnemyLayer);
    }

    public void DisableAttack()
    {
        enabled = false;
    }

    public void EnableAttack()
    {
        enabled = true;
    }
}

