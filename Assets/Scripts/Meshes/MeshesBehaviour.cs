#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class MeshesBehaviour : MonoBehaviour
{
    public MeshFilter meshFilter;

    [HideInInspector] public Mesh mesh;

    private Vector3[] vertices;

    [Range(0, 360)]
    public float amplitudeDegrees = 60;
    public float maxLenght = 3;
    public int rays = 4;


    private void Awake()
    {
        mesh = new Mesh();

        meshFilter.mesh = mesh;
    }


    private void Update()
    {
        vertices = new Vector3[rays + 1];
        var triangles = new int[(rays - 1) * 3];

        Vector3 origin = Vector3.zero;
        vertices[0] = origin;

        var halfAmplitude = amplitudeDegrees * .5f;
        var deltaAmpitude = amplitudeDegrees / (rays - 1);
        var originDirection = Quaternion.Euler(0, -halfAmplitude, 0) * transform.forward;

        for (int i = 1; i <= rays; i++)
        {
            var rotation = Quaternion.Euler
                (0, deltaAmpitude * (i - 1), 0);

            var direction = rotation * originDirection;

            var worldOrigin = transform.position + origin;

            if (Physics.Raycast(worldOrigin, direction, out RaycastHit hitInfo, maxLenght))
            {
                vertices[i] = hitInfo.point - transform.position;
            }
            else
            {
                vertices[i] = direction * maxLenght;
            }

        }


        for (int i = 0; i < rays - 1; i++)
        {
            triangles[i * 3] = 0;
            triangles[(i * 3) + 1] = i + 1;
            triangles[(i * 3) + 2] = i + 2;
        }


        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }


    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
            return;


        //Gizmos.DrawSphere(transform.position + vertices[0], .3f);
        //var halfAmplitude = amplitudeDegrees * .5f;
        //var deltaAmpitude = amplitudeDegrees / (rays - 1);
        //var originDirection = Quaternion.Euler(0, -halfAmplitude, 0) * transform.forward;
        //var finalDirection = Quaternion.Euler(0, halfAmplitude, 0) * transform.forward;

        //Gizmos.DrawLine(transform.position, transform.forward * maxLenght);
        ////Gizmos.DrawLine(transform.position, originDirection * maxLenght);
        ////Gizmos.DrawLine(transform.position, finalDirection * maxLenght);

        //Gizmos.color = Color.green;

        //for (int i = 0; i < rays; i++)
        //{
        //    var direction = Quaternion.Euler
        //        (0, deltaAmpitude * i, 0) * originDirection;

        //    var rayEnd = direction * maxLenght;

        //    Gizmos.DrawLine(transform.position, rayEnd + transform.position + transform.forward);
        //}
                

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            Handles.Label(transform.position + mesh.vertices[i], 
                mesh.vertices[i].ToString());

            Gizmos.DrawLine(transform.position, transform.position + vertices[i]);
        }
    }
}


public static class Meshes
{
    public static void DrawTriangle(
        Vector3 a, 
        Vector3 b, 
        Vector3 c, 
        Vector3[] vertices, 
        int[] triangles)
    {
        vertices[0] = a;
        vertices[1] = b;
        vertices[2] = c;

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
    }

    public static void DrawEquilateralTriangle(
        float sideWorldUnits, 
        Vector3[] vertices, int[] triangles)
    {
        var halfSide = sideWorldUnits * 0.5f;

        var h = 
            Mathf.Sqrt(
                Mathf.Pow(sideWorldUnits, 2) 
                - Mathf.Pow(halfSide, 2)
            );

        var a = Vector2.zero;
        var b = new Vector2(halfSide, h);
        var c = new Vector2(sideWorldUnits, 0);

        DrawTriangle(a, b, c, vertices, triangles);
    }

    public static void DrawQuad
        (
        Vector2 a, 
        Vector2 b,
        Vector2 c, 
        Vector2 d, 
        Vector3[] vertices, int[] triangles)
    {
        vertices[0] = a;
        vertices[1] = b;
        vertices[2] = c;
        vertices[3] = d;

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;
        triangles[3] = 3;
        triangles[4] = 1;
        triangles[5] = 2;
    }

    internal static void DrawQuad(
        float b, 
        float h, 
        Vector3[] vertices, int[] triangles)
    {
        Vector3 a = Vector3.zero;
        Vector3 _b = Vector3.up * h;
        Vector3 c = new Vector2(b, h);
        Vector3 d = Vector3.right * b;

        DrawQuad(a, _b, c, d, vertices, triangles);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(MeshesBehaviour))]
public class MeshesBehaviourEditor : Editor
{
    private void OnSceneGUI()
    {
        //if (Application.isPlaying == false)
        //    return;

        //MeshesBehaviour t = base.target as MeshesBehaviour;
        //var mesh = t.mesh;

        //for (int i = 0; i < mesh.vertices.Length; i++)
        //{
        //    Handles.Label(t.transform.position + mesh.vertices[i], mesh.vertices[i].ToString());
        //}

    }
}

#endif