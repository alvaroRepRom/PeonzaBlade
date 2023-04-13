using UnityEngine;

public class BladeController : MonoBehaviour, IDamagable
{
    [SerializeField] private CharacterStatsSO characterStatsSO;

    [SerializeField] private LayerMask groundMask;

    private Rigidbody rb;
    private BladeRotation    bladeRotation;
    private BladeInclination bladeInclination;
    private TurnCharacter    turnCharacter;

    [Header("Movement")]
    private float   moveSpeed;
    private float   initialRotationSpeed;
    private float   currentRotationSpeed;
    private Vector3 moveDirection;

    [Header("Actions")]
    private bool  canExecuteAction = true;
    private Timer actionTimer = new Timer( 1 );

    [Header("Attack")]
    private float secondsAttacking = 0.2f;
    private bool  isAttacking;

    [Header("Defense")]
    private float secondsDefending = 0.6f;
    private bool  isDefending;

    [Header("Jump")]
    private bool  isJumping;
    private float fallForceMultiplier = 1.8f;
    private bool  isGrounded = true;

    private const float MAX_INCLINATION_ANGLE = 33.5f;

    private GameInputs gameInputs;
    private SingleHUD singleHUD;
    private SoundEffects soundEffects;
    private BladeParticles bladeParticles;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bladeRotation    = GetComponentInChildren<BladeRotation>();
        bladeInclination = GetComponentInChildren<BladeInclination>();
        turnCharacter    = GetComponentInChildren<TurnCharacter>();
        soundEffects     = GetComponentInChildren<SoundEffects>();
        bladeParticles   = GetComponentInChildren<BladeParticles>();
    }

    public void SetGameInputs( GameInputs gameInputs )
    {
        this.gameInputs = gameInputs;
        Destroy ( GetComponent<CPUController>() );
    }

    public void SetSingleHUD( SingleHUD singleHUD )
    {
        this.singleHUD = singleHUD;
    }

    private void Start()
    {
        moveSpeed = characterStatsSO.movementSpeed;
        rb.mass   = characterStatsSO.weight;

        // This goes to update
        initialRotationSpeed = characterStatsSO.maxRotationSpeed;
        currentRotationSpeed = initialRotationSpeed;

        gameInputs.OnAttackPerformed  += GameInputs_OnAttackPerformed;
        gameInputs.OnDefensePerformed += GameInputs_OnDefensePerformed;
        gameInputs.OnJumpPerformed    += GameInputs_OnJumpPerformed;

        GameManager.Instance.OnWinnerDecided += SetThisAsWinner;
    }

    #region Input Callbacks
    private void GameInputs_OnJumpPerformed()
    {
        if ( !canExecuteAction ) return;

        canExecuteAction = false;
        isJumping   = true;
        isGrounded  = false;
        rb.velocity = new Vector3( rb.velocity.x , 0 , rb.velocity.z );
        rb.AddForce( characterStatsSO.jumpSpeed * Vector3.up , ForceMode.VelocityChange );
        soundEffects.PlayDash();
    }

    private void GameInputs_OnDefensePerformed()
    {
        if ( !canExecuteAction ) return;

        canExecuteAction = false;
        isDefending = true;
        actionTimer.SetNewTime( secondsDefending );
        rb.mass = characterStatsSO.defenseWeight;
        rb.velocity = Vector3.zero;
    }

    private void GameInputs_OnAttackPerformed()
    {
        if ( !canExecuteAction ) return;

        canExecuteAction = false;
        isAttacking = true;
        actionTimer.SetNewTime( secondsAttacking );
        rb.velocity = Vector3.zero;
        rb.AddForce( characterStatsSO.attackSpeed * moveDirection , ForceMode.VelocityChange );
        soundEffects.PlayDash();
    }
    #endregion

    private void Update()
    {
        MoveInput();
        AttackDash();
        DefenseAction();
        JumpControl();
        BalanceOverTime();
        OutOfBorders();

        bladeParticles.SetMoveParticles( !isJumping );

        turnCharacter.SetCharacterForwardDirection( moveDirection );
        SetBladePerpendicular();
        singleHUD.UpdateHUD( (int)currentRotationSpeed );
        CheckRotationHealth();
    }

    private void SetBladePerpendicular()
    {
        float hitDistance = 1;
        if ( Physics.Raycast( transform.position , -transform.up , out RaycastHit hit , hitDistance , groundMask ) )
        {
            float velocity = 0.1f;
            transform.up = Vector3.Lerp( transform.up , hit.normal , velocity );
        }
    }

    private void FixedUpdate()
    {
        FallGravity();
        Move();
    }

    private void Move()
    {
        rb.AddForce( moveSpeed * moveDirection , ForceMode.Force );
    }

    private void FallGravity()
    {
        if ( isJumping && rb.velocity.y < 0 )
            rb.AddForce( fallForceMultiplier * Vector3.down , ForceMode.Force );
    }

    private void MoveInput()
    {
        Vector2 moveInput = gameInputs.MovementNormalized();
        moveDirection     = new Vector3( moveInput.x , 0 , moveInput.y );
    }


    private void BalanceOverTime()
    {
        currentRotationSpeed -= Time.deltaTime * characterStatsSO.rotationUsedPerSecond;
        bladeRotation.SetRotationSpeed( currentRotationSpeed );

        bladeInclination.SetInclination( InclinationOverRotation() );
    }


    private float InclinationOverRotation()
    {
        return Mathf.Lerp( MAX_INCLINATION_ANGLE , 0 , currentRotationSpeed / characterStatsSO.maxRotationSpeed );
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
            rb.mass = characterStatsSO.weight;
        }
    }

    private void JumpControl()
    {
        if ( isJumping && isGrounded )
        {
            canExecuteAction = true;
            isJumping = false;
        }
    }

    private void OnCollisionEnter( Collision collision )
    {
        if ( collision.gameObject == gameObject ) return;

        if ( 1 << collision.gameObject.layer == groundMask.value )
        {
            isGrounded = true;
        }
        else
        if ( collision.gameObject.TryGetComponent( out IDamagable damagableRival ) )
        {
            soundEffects.PlayShock();
            bladeParticles.SetShockParticles();
            damagableRival.RecieveDamage( 
                isAttacking ? 
                characterStatsSO.dashAttackDamage : 
                characterStatsSO.normalAttackDamage 
            );
        }
    }

    private void CheckRotationHealth()
    {
        if ( currentRotationSpeed > 0 ) return;

        if ( GameManager.Instance )
        {
            int number = gameInputs.PlayerIndex + 1;
            GameManager.Instance.BladeHasDied( "Player " + number );
        }
        Destroy( gameObject );
    }


    private void OutOfBorders()
    {
        int autoDestroyDistance = 80;
        if ( Vector3.Distance( Vector3.zero , transform.position ) > autoDestroyDistance )
        {
            if ( GameManager.Instance )
            {
                int number = gameInputs.PlayerIndex + 1;
                GameManager.Instance.BladeHasDied( "Player " + number );
            }
            Destroy( gameObject );
        }
    }

    private void SetThisAsWinner()
    {
        int number = gameInputs.PlayerIndex + 1;
        GameManager.Instance.SetWinner( "Player " + number );
    }

    private void OnDestroy()
    {
        if ( GameManager.Instance )
            GameManager.Instance.OnWinnerDecided -= SetThisAsWinner;

        if ( gameInputs != null )
        {
            gameInputs.OnAttackPerformed  -= GameInputs_OnAttackPerformed;
            gameInputs.OnDefensePerformed -= GameInputs_OnDefensePerformed;
            gameInputs.OnJumpPerformed    -= GameInputs_OnJumpPerformed;
        }
    }

    public void RecieveDamage( float damage )
    {
        float damageReducer = isDefending ? characterStatsSO.specialDefense : characterStatsSO.normalDefense;
        currentRotationSpeed -= damage - damageReducer;
    }
}
