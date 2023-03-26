using UnityEngine;

public class BladeRotation : MonoBehaviour
{
    private float rotationSpeed = 3;

    private void Update()
    {
        transform.Rotate( Vector3.up * rotationSpeed , Space.World );
    }

    public void SetRotationSpeed( float speed ) 
    { 
        rotationSpeed = speed; 
    }
}
