using UnityEngine;

public class CPUController : MonoBehaviour, IDamagable
{
    [SerializeField] private CharacterStatsSO characterStatsSO;

    [SerializeField] private LayerMask groundMask;

    private Rigidbody rb;
    private BladeRotation bladeRotation;
    private BladeInclination bladeInclination;
    private TurnCharacter turnCharacter;
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
    private float fallForceMultiplier = 1.8f;

    private const float MAX_INCLINATION_ANGLE = 33.5f;

    private SingleHUD singleHUD;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bladeRotation    = GetComponentInChildren<BladeRotation>();
        bladeInclination = GetComponentInChildren<BladeInclination>();
        turnCharacter    = GetComponentInChildren<TurnCharacter>();
    }

    private void Start()
    {
        moveSpeed = characterStatsSO.movementSpeed;
        rb.mass   = characterStatsSO.weight;

        // This goes to update
        initialRotationSpeed = characterStatsSO.maxRotationSpeed;
        currentRotationSpeed = initialRotationSpeed;
    }

    private void Update()
    {
        BalanceOverTime();
        OutOfBorders();

        turnCharacter.SetCharacterForwardDirection( moveDirection );
        SetBladePerpendicular();
        singleHUD.UpdateHUD( (int)currentRotationSpeed );
        CheckRotationHealth();
    }

    private void FixedUpdate()
    {
        FallGravity();
        Move();
    }

    private void Move()
    {
        moveDirection = new Vector3( 0 , 0 , 0 );
        rb.AddForce( moveSpeed * moveDirection , ForceMode.Force );
    }

    private void FallGravity()
    {
        if ( rb.velocity.y < 0 )
            rb.AddForce( fallForceMultiplier * Vector3.down , ForceMode.Force );
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


    public void SetCPUBlade()
    {
        enabled = true;
        Destroy( GetComponent<BladeController>() );
    }

    public void SetSingleHUD( SingleHUD singleHUD )
    {
        this.singleHUD = singleHUD;
    }

    private void OutOfBorders()
    {
        int autoDestroyDistance = 200;
        if ( Vector3.Distance( Vector3.zero , transform.position ) > autoDestroyDistance )
            Destroy( gameObject );
    }

    public void RecieveDamage( float damage )
    {
        float damageReducer = isDefending ? characterStatsSO.specialDefense : characterStatsSO.normalDefense;
        currentRotationSpeed -= damage - damageReducer;
    }

    private void CheckRotationHealth()
    {
        if ( currentRotationSpeed > 0 ) return;

        Destroy( gameObject );
    }

    private void OnCollisionEnter( Collision collision )
    {
        if ( collision.gameObject == gameObject ) return;

        if ( collision.gameObject.TryGetComponent( out IDamagable damagableRival ) )
        {
            damagableRival.RecieveDamage(
                isAttacking ?
                characterStatsSO.dashAttackDamage :
                characterStatsSO.normalAttackDamage
            );
        }
    }
}
