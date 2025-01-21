using UnityEngine;

public class MaleEnemyPatrol : MonoBehaviour
{
    [Header("Patrol points")]
    [SerializeField] private Transform LeftEdge;
    [SerializeField] private Transform RightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform Enemy;
    [SerializeField] private float EnemySpeed;

    [Header("Enemy Animator")]
    [SerializeField] private Animator EnemyAnimator;

    private Vector3 InitScale;
    private bool IsMovingRight;
    private float IdleTimer;
    private readonly float IdleDuration = 1.5f;

    private void Awake()
    {
        InitScale = Enemy.localScale;
        IsMovingRight = true;
    }

    private void Update()
    {
        if (IsMovingRight)
        {
            if (Enemy.position.x <= RightEdge.position.x)
                MoveDirection(1);
            else
                ChangeDirection();
        }
        else
        {
            if (Enemy.position.x >= LeftEdge.position.x)
                MoveDirection(-1);
            else 
                ChangeDirection();
        }
    }

    private void MoveDirection(int _direction)
    {
        IdleTimer = 0;
        EnemyAnimator.SetBool("isWalking", true);

        Enemy.localScale = new Vector3(Mathf.Abs(InitScale.x) * _direction, InitScale.y, InitScale.z);
        Enemy.position = new Vector3(Enemy.position.x + Time.deltaTime * _direction * EnemySpeed, Enemy.position.y, Enemy.position.z);
    }

    public void ChangeDirection()
    {
        EnemyAnimator.SetBool("isWalking", false);
        IdleTimer += Time.deltaTime;

        if (IdleTimer >= IdleDuration)
        {
            IsMovingRight = !IsMovingRight;
        }
    }


    private void OnDisable()
    {
        EnemyAnimator.SetBool("isWalking", false);
    }
}

