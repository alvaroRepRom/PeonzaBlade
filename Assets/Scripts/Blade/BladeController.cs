using UnityEngine;

public class BladeController : MonoBehaviour
{
    [SerializeField] private CharacterStatsSO characterStatsSO;

    private CharacterStats characterStats;
    private Rigidbody rb;
    private BladeRotation bladeRotation;
    private BladeInclination bladeInclination;
    private float gameTime;

    [Header("Movement")]
    private float moveSpeed;
    private float initialRotationSpeed;
    private float currentRotationSpeed;
    private Vector3 moveDirection;

    [Header("Actions")]
    private bool canExecuteAction = true;
    private Timer actionTimer = new Timer( 1 );

    [Header("Attack")]
    private float secondsAttacking = 0.2f;
    private bool isAttacking;

    [Header("Defense")]
    private float secondsDefending = 0.6f;
    private bool isDefending;

    [Header("Jump")]
    private bool isJumping;


    private const float MAX_INCLINATION_ANGLE = 33.5f;


    private void Awake()
    {
        characterStats = new CharacterStats( characterStatsSO );

        rb = GetComponent<Rigidbody>();
        bladeRotation = GetComponentInChildren<BladeRotation>();
        bladeInclination = GetComponentInChildren<BladeInclination>();
    }

    private void Start()
    {
        moveSpeed = characterStats.movementSpeed;
        rb.mass = characterStats.weight;

        // This goes to update
        initialRotationSpeed = characterStats.maxRotationSpeed;
        currentRotationSpeed = initialRotationSpeed;


        gameTime = GameManager.Instance.GameTime();


        GameInputs.Instance.OnAttackPerformed += GameInputs_OnAttackPerformed;
        GameInputs.Instance.OnDefensePerformed += GameInputs_OnDefensePerformed;
        GameInputs.Instance.OnJumpPerformed += GameInputs_OnJumpPerformed;
    }

    private void GameInputs_OnJumpPerformed()
    {
        if ( canExecuteAction )
        {
            isJumping = true;
            rb.AddForce( characterStats.jumpSpeed * Vector3.up , ForceMode.VelocityChange );
        }
    }

    private void GameInputs_OnDefensePerformed()
    {
        if ( canExecuteAction )
        {
            canExecuteAction = false;
            isDefending = true;
            actionTimer.SetNewTime( secondsDefending );
            rb.mass = characterStats.defenseWeight;
            rb.velocity = Vector3.zero;
        }
    }

    private void GameInputs_OnAttackPerformed()
    {
        if ( canExecuteAction )
        {
            canExecuteAction = false;
            isAttacking = true;
            actionTimer.SetNewTime( secondsAttacking );
            rb.velocity = Vector3.zero;
            rb.AddForce( characterStats.attackSpeed * moveDirection , ForceMode.VelocityChange );
        }
    }

    private void Update()
    {
        MoveInput();
        AttackDash();
        DefenseAction();
        JumpControl();
        BalanceOverTime();
    }

    private void FixedUpdate()
    {
        rb.AddForce( moveSpeed * moveDirection , ForceMode.Force );
    }

    private void MoveInput()
    {
        Vector2 moveInput = GameInputs.Instance.MovementNormalized();
        moveDirection = new Vector3( moveInput.x , 0 , moveInput.y );
    }


    private void BalanceOverTime()
    {
        currentRotationSpeed = RotationOverTime();
        bladeRotation.SetRotationSpeed( currentRotationSpeed );

        bladeInclination.SetInclination( InclinationOverRotation() );
    }

    private float RotationOverTime()
    {
        return Mathf.Lerp( initialRotationSpeed , 0 , GameManager.Instance.TimeElapsed() / gameTime );
    }


    private float InclinationOverRotation()
    {
        return Mathf.Lerp( MAX_INCLINATION_ANGLE , 0 , currentRotationSpeed / characterStats.maxRotationSpeed );
    }



    private void AttackDash()
    {
        if ( isAttacking && actionTimer.HasTimeUp() )
        {
            canExecuteAction = true;
            isAttacking = false;
        }
    }


    private void DefenseAction()
    {
        if ( isDefending && actionTimer.HasTimeUp() )
        {
            canExecuteAction = true;
            isDefending = false;
            rb.mass = characterStats.weight;
        }
    }

    private void JumpControl()
    {
        if ( isJumping && IsGrounded() )
        {
            canExecuteAction = true;
            isJumping = false;
        }
    }

    private bool IsGrounded()
    {
        return true;
    }


    private void OnDestroy()
    {
        GameInputs.Instance.OnAttackPerformed -= GameInputs_OnAttackPerformed;
        GameInputs.Instance.OnDefensePerformed -= GameInputs_OnDefensePerformed;
        GameInputs.Instance.OnJumpPerformed -= GameInputs_OnJumpPerformed;
    }

}
