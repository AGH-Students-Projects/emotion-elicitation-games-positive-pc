using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    [Header("Player movement")]
    [SerializeField] private float runMaxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float accelerationInAir;
    [SerializeField] private float decceleration;
    [SerializeField] private float deccelerationInAir;
    [SerializeField] private float velocityPower;
    [SerializeField] private float friction;
    [SerializeField] private bool doConserveMomentum;

    [Space(5)]

    [Header("Player jump")]
    [SerializeField] private float gravityScale;
    [SerializeField] private float jumpPower;
    [SerializeField] private float jumpMaxFallSpeed;
    [SerializeField] private float fastJumpMaxFallSpeed;
    [SerializeField] private float jumpCutGravityMultiplier;
    [SerializeField] private float jumpGravityMultiplier;
    [SerializeField] private float jumpHangGravityMultiplier;
    [SerializeField] private float jumpFallGravityMultiplier;
    [SerializeField] private float jumpHangAccelerationMultiplier;
    [SerializeField] private float jumpHangMaxSpeedMultiplier;

    [SerializeField] private float jumpCayoteTime;
    [SerializeField] private float jumpBufferTime;
    [SerializeField] private float jumpHangTimeThreshold;

    [Space(5)]
    [Header("Player dash")]
    [SerializeField] private float DashTime;
    [SerializeField] private float DashPower;
    [SerializeField] private float DashCooldown;

    [Space(5)]
    [Header("Player Sounds")]
    [SerializeField] private AudioClip JumpSound;
    [SerializeField] private AudioClip DashSound;
    [SerializeField] private AudioClip LandSound;
    [SerializeField] private AudioClip RunSound;

    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask enemyLayer;

    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    private Animator PlayerAnimator;
    private TrailRenderer trailRenderer;
    private PlayerHealth playerHealth;

    // Movement
    private Vector2 moveInput;
    public bool IsFacingRight { get; private set; }
    public bool IsSprinting { get; private set; }

    // Jump
    public bool IsJumping { get; private set; }
    public bool IsJumpFalling { get; private set; }
    public bool IsJumpCut { get; private set; }

    // Dash
    public bool CanDash { get; private set; }
    public bool IsDashing { get; private set; }
    public float DashTimeLeft { get; private set; }
    public float LastDashTime { get; private set; }
    public Vector2 DashDirection { get; private set; }

    // Ladder
    public bool IsClimbing { get; private set; }
    public bool IsWithinLadder { get; private set; }

    // Timers
    public float LastGroundedTime { get; private set; }
    public float LastJumpTime { get; private set; }
    public float MaxFallTime { get; private set; } = 2f;


    private void Awake()
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

        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        PlayerAnimator = GetComponent<Animator>();
        trailRenderer = GetComponent<TrailRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForInputManager());
    }

    private IEnumerator WaitForInputManager()
    {
        while (PlayerInputManager.Instance == null)
        {
            yield return null; // Wait for the next frame
        }

        var inputActions = PlayerInputManager.Instance.InputActions;

        inputActions.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>().normalized;
        inputActions.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Gameplay.Jump.performed += ctx => JumpKeyDownInput();
        inputActions.Gameplay.Jump.canceled += ctx => JumpKeyUpInput();
        inputActions.Gameplay.Dash.performed += ctx => Dash();
    }


    private void OnDisable()
    {
        var inputActions = PlayerInputManager.Instance.InputActions;

        inputActions.Gameplay.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Gameplay.Move.canceled -= ctx => moveInput = Vector2.zero;

        inputActions.Gameplay.Jump.performed -= ctx => JumpKeyDownInput();
        inputActions.Gameplay.Jump.canceled -= ctx => JumpKeyUpInput();
        inputActions.Gameplay.Dash.performed -= ctx => Dash();
    }

    private void Start()
    {
        SetGravityScale(gravityScale);
        IsFacingRight = true;
    }

    void Update()
    {
        #region Timers

        LastGroundedTime -= Time.deltaTime;
        LastJumpTime -= Time.deltaTime;

        #endregion

        #region FallingTimeCheck

        if (MaxFallTime <= 0)
        {
            playerHealth.TakeDamage(3);
        }

        #endregion

        #region Movement Input

        if (moveInput.x != 0)
            CheckPlayerFacingDirection(moveInput.x > 0);

        #endregion

        #region Dash

        if (IsDashing)
        {
            if (DashTimeLeft > 0)
            {
                DashTimeLeft -= Time.deltaTime;
            }
            else
            {
                EndDash();
            }
        }
        #endregion


        #region Ladder climbing

        if (IsWithinLadder)
        {
            rigidbody2D.gravityScale = 0;
            rigidbody2D.velocity = Vector2.zero;

            if (moveInput.y != 0)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, moveInput.y * runMaxSpeed);
            }
        }
        else
        {
            rigidbody2D.gravityScale = gravityScale;
        }

        #endregion

        #region Ground Collision Check

        if (!IsJumping && IsGrounded())
        {
            LastGroundedTime = jumpCayoteTime;
            MaxFallTime = 2f;
        }

        #endregion

        #region Jump Checks

        if (IsJumping && rigidbody2D.velocity.y < 0)
        {
            IsJumping = false;
            IsJumpFalling = true;
            StopEmittingTrail();
        }

        if (LastGroundedTime > 0 && !IsJumping)
        {
            IsJumpCut = false;

            if (!IsJumping)
                IsJumpFalling = false;
        }

        if (rigidbody2D.velocity.y < 0)
        {
            MaxFallTime -= Time.deltaTime;
        }

        #endregion

        #region Jump

        if (CanJump() && LastJumpTime > 0)
        {
            IsJumping = true;
            IsJumpCut = false;
            IsJumpFalling = false;
            Jump();
        }

        #endregion

        #region Gravity

        if (rigidbody2D.velocity.y < 0 && moveInput.y < 0)
        {
            SetGravityScale(gravityScale * jumpGravityMultiplier);
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, Mathf.Max(rigidbody2D.velocity.y, -fastJumpMaxFallSpeed));
        }
        else if (IsJumpCut)
        {
            SetGravityScale(gravityScale * jumpCutGravityMultiplier);
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, Mathf.Max(rigidbody2D.velocity.y, -jumpMaxFallSpeed));
        }
        else if ((IsJumping || IsJumpFalling) && Mathf.Abs(rigidbody2D.velocity.y) < jumpHangTimeThreshold)
        {
            SetGravityScale(gravityScale * jumpHangGravityMultiplier);
        }
        else if (rigidbody2D.velocity.y < 0)
        {
            SetGravityScale(gravityScale * jumpFallGravityMultiplier);
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, Mathf.Max(rigidbody2D.velocity.y, -jumpMaxFallSpeed));
        }
        else
        {
            SetGravityScale(gravityScale);
        }

        #endregion

        #region Animators

        PlayerAnimator.SetBool("isMovingOnTheGround", IsGrounded() && moveInput.x != 0);
        PlayerAnimator.SetBool("startedJumping", rigidbody2D.velocity.y > 0 && IsJumping && !IsJumpCut && !IsJumpFalling && !playerHealth.IsDead);
        PlayerAnimator.SetBool("isFallingDown", (rigidbody2D.velocity.y <= 0 || IsJumpFalling) && !IsGrounded() && !playerHealth.IsDead);
        PlayerAnimator.SetBool("isGrounded", IsGrounded());
        PlayerAnimator.SetBool("isDashing", IsDashing);

        #endregion
    }

    private void FixedUpdate()
    {
        Run(2);
    }

    private void SetGravityScale(float scale)
    {
        rigidbody2D.gravityScale = scale;
    }

    private void JumpKeyDownInput()
    {
        LastJumpTime = jumpBufferTime;
    }

    private void JumpKeyUpInput()
    {
        if (CanJumpCut()) IsJumpCut = true;
    }

    private bool CanJumpCut()
    {
        return IsJumping && rigidbody2D.velocity.y > 0;
    }

    private bool CanJump()
    {
        return LastGroundedTime > 0 && !IsJumping;
    }

    private void Jump()
    {
        LastJumpTime = 0;
        LastGroundedTime = 0;

        SoundManager.Instance.PlaySound(JumpSound);

        float force = jumpPower;

        if (rigidbody2D.velocity.y < 0)
            force -= rigidbody2D.velocity.y;

        StartEmittingTrail();
        rigidbody2D.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private void Run(float lerpAmount)
    {
        float targetSpeed;
        float accelerationRate;

        targetSpeed = moveInput.x * runMaxSpeed;
        targetSpeed = Mathf.Lerp(rigidbody2D.velocity.x, targetSpeed, lerpAmount);

        if (LastGroundedTime > 0)
            accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        else
            accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration * accelerationInAir : decceleration * deccelerationInAir;

        if ((IsJumping || IsJumpFalling) && Mathf.Abs(rigidbody2D.velocity.y) < jumpHangTimeThreshold)
        {
            accelerationRate *= jumpHangAccelerationMultiplier;
            targetSpeed *= jumpHangMaxSpeedMultiplier;
        }

        if (doConserveMomentum &&
            Mathf.Abs(rigidbody2D.velocity.x) > Mathf.Abs(targetSpeed) &&
            Mathf.Sign(rigidbody2D.velocity.x) == Mathf.Sign(targetSpeed) &&
            Mathf.Abs(targetSpeed) > 0.01f &&
            LastGroundedTime < 0)
        {
            accelerationRate = 0;
        }

        float speedDif = targetSpeed - rigidbody2D.velocity.x;
        float movement = speedDif * accelerationRate;

        rigidbody2D.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void Dash()
    {
        if (rigidbody2D == null) return; // Early exit if rigidbody2D is missing or destroyed
        if (IsDashing || Time.time - LastDashTime < DashCooldown) return;

        IsDashing = true;
        CanDash = false;

        SoundManager.Instance?.PlaySound(DashSound);

        DashTimeLeft = DashTime;
        LastDashTime = Time.time;

        DashDirection = IsFacingRight ? Vector2.right : Vector2.left;

        rigidbody2D.AddForce(DashDirection * DashPower, ForceMode2D.Impulse);

        StartEmittingTrail();
    }


    private void EndDash()
    {
        IsDashing = false;
        rigidbody2D.velocity = Vector2.zero;

        Invoke(nameof(ResetDash), DashCooldown);

        StopEmittingTrail();
    }

    private void ResetDash()
    {
        CanDash = true;
    }

    private void CheckPlayerFacingDirection(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }

    private void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (IsFacingRight ? -1 : 1);
        transform.localScale = scale;
        IsFacingRight = !IsFacingRight;
    }

    public bool IsGrounded()
    {
        // Use the absolute value of scale to ensure consistent box size
        Vector2 boxSize = new(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, 0.1f, groundLayer);

        return hit.collider != null;
    }

    public bool IsOnEnemy()
    {
        Vector2 boxSize = new(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, 0.1f, enemyLayer);

        return hit.collider != null && hit.collider.CompareTag("Enemy");
    }

    public bool CanAttack()
    {
        return IsGrounded();
    }

    private void StartEmittingTrail()
    {
        trailRenderer.emitting = true;
    }

    private void StopEmittingTrail()
    {
        trailRenderer.emitting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            LastGroundedTime = jumpCayoteTime;
            MaxFallTime = 2f;
            IsWithinLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            IsWithinLadder = false;
            IsClimbing = false;
        }
    }

    public void SetAnimator()
    {
        PlayerAnimator.SetBool("isMovingOnTheGround", false);
        PlayerAnimator.SetBool("startedJumping", false);
        PlayerAnimator.SetBool("isFallingDown", false);

        PlayerAnimator.SetTrigger("isIdle");
    }
}
