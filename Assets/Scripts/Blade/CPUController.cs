using UnityEngine;

public class CPUController : MonoBehaviour, IDamagable
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
    private Timer actionsTimer = new Timer( 1 );

    [Header("Attack")]
    private bool  isAttacking;

    [Header("Defense")]
    private bool  isDefending;

    [Header("Jump")]
    private float fallForceMultiplier = 1.8f;

    private int       pointIndex;
    private Vector3[] wayPoints = {
        new Vector3(   0 , 0 ,   0 ),
        new Vector3(  10 , 0 ,   0 ),
        new Vector3( -10 , 0 ,   0 ),
        new Vector3(   0 , 0 ,  10 ),
        new Vector3(   0 , 0 , -10 ),
        new Vector3(   8 , 0 ,   8 ),
        new Vector3(   8 , 0 ,  -8 ),
        new Vector3(  -8 , 0 ,   8 ),
        new Vector3(  -8 , 0 ,  -8 )
    };


    private const float MAX_INCLINATION_ANGLE = 33.5f;

    private SingleHUD singleHUD;
    private int CPUNumber;

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
        pointIndex = Random.Range( 0, wayPoints.Length );

        // This goes to update
        initialRotationSpeed = characterStatsSO.maxRotationSpeed;
        currentRotationSpeed = initialRotationSpeed;

        GameManager.Instance.OnWinnerDecided += SetThisAsWinner;
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
        moveDirection = MoveDirectionNormalized();
        rb.AddForce( moveSpeed * moveDirection , ForceMode.Force );
    }

    private Vector3 MoveDirectionNormalized()
    {
        Vector2 localPos = new Vector2( transform.position.x , transform.position.z );
        Vector2 point    = new Vector2( wayPoints[pointIndex].x , wayPoints[pointIndex].z );

        if ( Vector2.Distance( localPos , point ) < 0.4f )
            pointIndex = Random.Range( 0 , wayPoints.Length );

        if ( rb.velocity.magnitude < 0.1f && actionsTimer.HasTimeUp() )
            pointIndex = Random.Range( 0 , wayPoints.Length );

        float x = wayPoints[pointIndex].x - localPos.x;
        float z = wayPoints[pointIndex].z - localPos.y;
        Vector3 direction = new Vector3( x , 0 , z );

        return direction.normalized;
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


    public void SetCPUBlade( int cpuNumber )
    {
        CPUNumber = cpuNumber;
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
        {
            if ( GameManager.Instance )
                GameManager.Instance.BladeHasDied( "CPU " + CPUNumber );
            Destroy( gameObject );
        }
    }

    public void RecieveDamage( float damage )
    {
        float damageReducer = isDefending ? characterStatsSO.specialDefense : characterStatsSO.normalDefense;
        currentRotationSpeed -= damage - damageReducer;
    }

    private void CheckRotationHealth()
    {
        if ( currentRotationSpeed > 0 ) return;

        if ( GameManager.Instance )
            GameManager.Instance.BladeHasDied( "CPU " + CPUNumber );
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
            pointIndex = Random.Range( 0 , wayPoints.Length );
        }
    }

    private void SetThisAsWinner() => GameManager.Instance.SetWinner( "CPU " + CPUNumber );

    private void OnDestroy()
    {
        if ( GameManager.Instance )
            GameManager.Instance.OnWinnerDecided -= SetThisAsWinner;
    }
}
