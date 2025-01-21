using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{

    [Header("Enemy Attack")]
    [SerializeField] private float AttackCooldown;
    [SerializeField] private float AttackDamage;
    [SerializeField] private float AttackRange;

    [Header("Enemy Vision")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform EnemyAttackPoint;

    [Header("Enemy Sound")]
    //[SerializeField] private AudioClip ScareSound;
    [SerializeField] private AudioClip AttackSound;

    private float cooldownTimer = Mathf.Infinity;
    private readonly float VisionRange = 2.5f;

    private BoxCollider2D EnemyCollider;
    private Animator EnemyAnimator;
    private PlayerHealth playerHealth;
    private MaleEnemyPatrol EnemyPatrol;
    private EnemyHealth EnemyHealth;

    void Awake()
    {
        EnemyHealth = GetComponent<EnemyHealth>();
        EnemyCollider = GetComponent<BoxCollider2D>();
        EnemyAnimator = GetComponent<Animator>();
        EnemyPatrol = GetComponentInParent<MaleEnemyPatrol>();
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            //SoundManager.Instance.PlaySound(ScareSound);

            if (cooldownTimer >= AttackCooldown && PlayerInAttackRange())
            {
                cooldownTimer = 0;
                EnemyAnimator.SetTrigger("Attack");
                SoundManager.Instance.PlaySound(AttackSound);
            }
        }

        if (EnemyPatrol != null)
            EnemyPatrol.enabled = !PlayerInSight() && !EnemyHealth.IsDead;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            EnemyCollider.bounds.center + transform.localScale.x * VisionRange * transform.right,
            new Vector3(EnemyCollider.bounds.size.x * VisionRange, EnemyCollider.bounds.size.y, EnemyCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.collider.GetComponent<PlayerHealth>();

        return hit.collider != null;
    }

    private void DamagePlayer()
    {
        if (PlayerInSight() && PlayerInAttackRange())
        {
            playerHealth.TakeDamage(AttackDamage);
        }
    }

    private bool PlayerInAttackRange()
    {
        Collider2D hit = Physics2D.OverlapCircle(EnemyAttackPoint.position, AttackRange, playerLayer);

        return hit != null;
    }
}
