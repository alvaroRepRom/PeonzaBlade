using UnityEngine;

public class BladeInclination : MonoBehaviour
{
    public void SetInclination( float inclinationAngle ) 
    { 
        transform.localEulerAngles = new Vector3( 0 , 0 , inclinationAngle );
    }
}
