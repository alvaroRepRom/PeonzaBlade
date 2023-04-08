using UnityEngine;

public class TurnCharacter : MonoBehaviour
{
    public void SetCharacterForwardDirection( Vector3 forwardDirection )
    {
        if ( forwardDirection == Vector3.zero ) return;
        transform.localRotation = Quaternion.LookRotation( forwardDirection );
    }
}
