using UnityEngine;

public class BladeController : MonoBehaviour
{
    [SerializeField] private CharacterStatsSO characterStatsSO;

    private CharacterStats characterStats;
    private Rigidbody rb;
    private BladeRotation bladeRotation;
    private BladeInclination bladeInclination;

    private float moveSpeed;
    private float initialRotationSpeed;
    private float currentRotationSpeed;

    private float gameTime;

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

        initialRotationSpeed = characterStats.maxRotationSpeed;
        currentRotationSpeed = initialRotationSpeed;

        gameTime = GameManager.Instance.GameTime();
    }

    private void Update()
    {
        Movement();

        BalanceOverTime();
    }

    private void Movement()
    {
        Vector2 moveInput = GameInputs.Instance.MovementNormalized();
        Vector3 moveDirection = new Vector3( moveInput.x , 0 , moveInput.y );

        rb.MovePosition( rb.position + Time.deltaTime * moveSpeed * moveDirection );
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


}
