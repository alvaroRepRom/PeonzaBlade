using UnityEngine;

public class CPUController : MonoBehaviour, IDamagable
{
    [SerializeField] private CharacterStatsSO characterStatsSO;

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

    private float timeRotating = 30f;

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

    private void Update()
    {
        OutOfBorders();
        singleHUD.UpdateHUD( (int)currentRotationSpeed );
    }


    public void SetCPUBlade()
    {
        enabled = true;
        GetComponent<BladeController>().enabled = false;
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
        Debug.Log( "Attack: " + damage + ", defense: " + damageReducer +
            ", currentRotation: " + currentRotationSpeed + ", cpu");
    }

    private void OnCollisionEnter( Collision collision )
    {
        if ( collision.gameObject == gameObject ) return;

        if ( collision.gameObject.TryGetComponent( out IDamagable damagable ) )
        {
            damagable.RecieveDamage(
                isAttacking ?
                characterStatsSO.dashAttackDamage :
                characterStatsSO.normalAttackDamage
            );
        }
    }
}
