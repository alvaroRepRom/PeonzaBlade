using UnityEngine;

public class BladeInclination : MonoBehaviour
{
    private float inclinationAngle = 0;

    private void Update()
    {
        transform.eulerAngles = new Vector3( 0 , 0 , inclinationAngle );
    }

    public void SetInclination( float inclinationAngle ) 
    { 
        this.inclinationAngle = inclinationAngle; 
    }
}
