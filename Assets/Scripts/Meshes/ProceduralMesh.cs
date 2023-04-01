using UnityEngine;

public class ProceduralMesh : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Mesh mesh;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;
    }

    private void Update()
    {
        DinamicMesh();
    }

    private void DinamicMesh()
    {
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        for ( var i = 0; i < vertices.Length; i++ )
        {
            vertices[i] += normals[i] * Mathf.Sin( Time.time );
        }

        mesh.vertices = vertices;
    }
}
