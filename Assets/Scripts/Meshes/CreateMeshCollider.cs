using UnityEngine;

public class CreateMeshCollider : MonoBehaviour
{
    private MeshCollider meshCollider;

    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
    }

    public void SetCollider( Mesh mesh )
    {
        //gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
    }
}
