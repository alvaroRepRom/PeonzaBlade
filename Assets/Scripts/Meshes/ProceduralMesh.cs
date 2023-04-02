using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ProceduralMesh : MonoBehaviour
{
    [SerializeField] private int xSize = 20;
    [SerializeField] private int zSize = 20;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;


    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
        GetComponent<CreateMeshCollider>().SetCollider( mesh );
    }

    private void CreateShape()
    {
        vertices = new Vector3[( xSize + 1 ) * ( zSize + 1 )];

        for ( int i = 0, z = 0; z <= zSize; z++ )
        {
            for( int x = 0; x <= xSize; x++ )
            {
                float y = Mathf.PerlinNoise( x * .3f, z * .3f ) * 2f;
                vertices[i] = new Vector3( x, y , z );
                i++;
            }
        }


        triangles = new int[xSize * zSize * 6];

        int vertex = 0; // vertices  count
        int tris = 0;   // triangles count

        for ( int z = 0; z < zSize; z++ )
        {
            for ( int x = 0; x < xSize; x++ )
            {
                triangles[tris]     = vertex;
                triangles[tris + 1] = vertex + xSize + 1;
                triangles[tris + 2] = vertex + 1;

                triangles[tris + 3] = vertex + 1;
                triangles[tris + 4] = vertex + xSize + 1;
                triangles[tris + 5] = vertex + xSize + 2;

                vertex++;
                tris += 6;
            }
            vertex++;
        }
    }
    
    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
