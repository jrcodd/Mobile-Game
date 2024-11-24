using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TriangleMesh : MonoBehaviour
{
    public bool isUpsideDown;
    public float thickness = 0.1f;

    void Awake()
    {
        GenerateMesh();
    }

    void OnValidate()
    {
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices;
        if (isUpsideDown)
        {
            vertices = new Vector3[]
            {
                // Front face
                new Vector3(0, -0.5f, 0),
                new Vector3(-0.5f, 0.5f, 0),
                new Vector3(0.5f, 0.5f, 0),
                // Back face
                new Vector3(0, -0.5f, thickness),
                new Vector3(-0.5f, 0.5f, thickness),
                new Vector3(0.5f, 0.5f, thickness)
            };
        }
        else
        {
            vertices = new Vector3[]
            {
                // Front face
                new Vector3(0, 0.5f, 0),
                new Vector3(-0.5f, -0.5f, 0),
                new Vector3(0.5f, -0.5f, 0),
                // Back face
                new Vector3(0, 0.5f, thickness),
                new Vector3(-0.5f, -0.5f, thickness),
                new Vector3(0.5f, -0.5f, thickness)
            };
        }

        int[] triangles = new int[]
        {
            // Front face
            0, 1, 2,
            // Back face
            3, 5, 4,
            // Sides
            0, 3, 1, 1, 3, 4, // Side 1
            1, 4, 2, 2, 4, 5, // Side 2
            2, 5, 0, 0, 5, 3  // Side 3
        };

        Vector2[] uvs = new Vector2[]
        {
            new Vector2(0.5f, 1),
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0.5f, 1),
            new Vector2(0, 0),
            new Vector2(1, 0)
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }
}
