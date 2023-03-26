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
    private float rotationSpeed;
    private float inclinationAngle = 0f;

    private float gameTime;
    private float gameElapsedSeconds;

    private const int MAX_INCLINATION_ANGLE = 60;


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
        rotationSpeed = initialRotationSpeed;

        gameTime = GameManager.Instance.GameTime();
    }

    private void Update()
    {
        inclinationAngle = 0;
        gameElapsedSeconds = GameManager.Instance.TimeElapsed();


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
        rotationSpeed -= RotationOverTime();
        bladeRotation.SetRotationSpeed( rotationSpeed );

        inclinationAngle += InclinationOverRotation();
        bladeInclination.SetInclination( inclinationAngle );
    }

    private float InclinationOverRotation()
    {
        return Mathf.Lerp( 0 , MAX_INCLINATION_ANGLE , rotationSpeed / characterStats.maxRotationSpeed );
    }


    private float RotationOverTime()
    {
        return Mathf.Lerp( 0 , MAX_INCLINATION_ANGLE , gameElapsedSeconds / gameTime );
    }
}
