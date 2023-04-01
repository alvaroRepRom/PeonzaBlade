using UnityEngine;

public class CPUController : MonoBehaviour
{
    [SerializeField] private CharacterStatsSO characterStatsSO;


    private void Update()
    {
        OutOfBorders();
    }


    public void SetCPUBlade()
    {
        enabled = true;
        GetComponent<BladeController>().enabled = false;
    }

    private void OutOfBorders()
    {
        int autoDestroyDistance = 200;
        if ( Vector3.Distance( Vector3.zero , transform.position ) > autoDestroyDistance )
            Destroy( gameObject );
    }
}
