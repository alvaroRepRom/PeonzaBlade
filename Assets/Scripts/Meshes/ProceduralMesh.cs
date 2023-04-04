using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ProceduralMesh : MonoBehaviour
{
    [SerializeField] private int xSize = 20;
    [SerializeField] private int zSize = 20;
    [SerializeField][Range(0.01f, 1)] private float cellSize = 0.5f;
    [SerializeField] private bool createMesh;
    [SerializeField] private float radius = 15;
    [SerializeField] private float height = 6;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;


    private void Start()
    {
        if ( createMesh )
        {
            CreateMesh();
            UpdateMesh();
        }
        else
        {
            CloneMesh();
        }

        MoveVertices();
        UpdateMesh();
        

        gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void CloneMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh originalMesh     = meshFilter.sharedMesh;

        mesh = new Mesh();
        mesh.name       = "clone " + originalMesh.name;
        mesh.vertices   = originalMesh.vertices;
        mesh.triangles  = originalMesh.triangles;
        mesh.normals    = originalMesh.normals;
        mesh.uv         = originalMesh.uv;

        meshFilter.mesh = mesh;

        vertices  = mesh.vertices;
        triangles = mesh.triangles;
    }

    private void MoveVertices()
    {
        vertices = mesh.vertices;
        triangles = mesh.triangles;

        Vector3 sphereOrigin = new Vector3( 0 , height , 0 );

        for ( int i = 0; i < mesh.vertexCount; i++ )
        {
            if ( Vector3.Distance( vertices[i] , sphereOrigin ) > radius )
                continue;

            float x = vertices[i].x;
            float z = vertices[i].z;
            float horizontal = Mathf.Sqrt( x * x + z * z );
            float angle = Mathf.Atan2( height , horizontal );

            float dhorizontal = Mathf.Cos( angle ) * radius - horizontal;
            float dy = Mathf.Sin( angle ) * radius - height;
            //Debug.Log( dy );

            float horizontalAngle = Mathf.Atan2( z , x);
            float dx = dhorizontal * Mathf.Cos( horizontalAngle );
            float dz = dhorizontal * Mathf.Sin( horizontalAngle );

            vertices[i] = new Vector3( x + dx , vertices[i].y - dy , z + dz );
        }
    }

    private void CreateMesh()
    {
        mesh = new Mesh();
        mesh.name = "MyMesh";
        GetComponent<MeshFilter>().mesh = mesh;


        uvs = new Vector2[( xSize + 1 ) * ( zSize + 1 )];
        vertices = new Vector3[( xSize + 1 ) * ( zSize + 1 )];
        int xHalfSize = xSize / 2;
        int zHalfSize = zSize / 2;

        for ( int i = 0, z = 0; z <= zSize; z++ )
        {
            for( int x = 0; x <= xSize; x++ )
            {
                vertices[i] = new Vector3( x - xHalfSize , 0 , z - zHalfSize ) * cellSize;
                uvs[i]      = new Vector2( ( float )x / xSize , ( float )z / zSize );
                i++;
            }
        }


        triangles = new int[xSize * zSize * 6];

        int vertex = 0; // vertices  count
        int tris   = 0; // triangles count

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

        mesh.vertices  = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        mesh.RecalculateUVDistributionMetrics();
    }
}
