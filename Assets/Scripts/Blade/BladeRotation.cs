using UnityEngine;

public class BladeRotation : MonoBehaviour
{
    public void SetRotationSpeed( float speed ) 
    {
        transform.Rotate( Vector3.up * speed , Space.Self );
    }
}
