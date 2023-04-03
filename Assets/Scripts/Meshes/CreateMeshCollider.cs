using UnityEngine;

[RequireComponent( typeof( MeshCollider ) )]
public class CreateMeshCollider : MonoBehaviour
{
    private MeshCollider meshCollider;

    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
    }

    public void SetCollider( Mesh mesh )
    {
        meshCollider.sharedMesh = mesh;
    }
}
