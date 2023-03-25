using UnityEngine;

public class BladeController : MonoBehaviour
{
    [SerializeField] private float speed = 3;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        Vector2 moveInput = GameInputs.Instance.MovementNormalized();
        Vector3 moveDirection = new Vector3( moveInput.x , 0 , moveInput.y );

        rb.MovePosition( rb.position + moveDirection * Time.deltaTime * speed );
    }
}
