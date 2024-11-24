using UnityEngine;

[ExecuteInEditMode]
public class WallGenerator : MonoBehaviour
{
    public GameObject trianglePrefab;
    public GameObject upsideDownTrianglePrefab;
    public int rows = 5;
    public int columns = 5;
    public float triangleWidth = 1.0f;
    public float triangleHeight = 1.0f;
    public float triangleThickness = 0.1f;


    void Start()
    {
        GenerateWallWithHole();
    }

    void GenerateWallWithHole()
    {
        // Clean up previous triangles
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        // Determine the total number of triangles
        int totalTriangles = rows * columns;

        // Select a random position for the hole
        int holeIndex = Random.Range(0, totalTriangles);

        // Iterate through each position in the grid
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Calculate the index in the grid
                int index = i * columns + j;

                // Skip the position chosen for the hole
                if (index == holeIndex)
                {
                    continue;
                }
                float xPos = 0.5f * j * triangleWidth;
                float yPos = 0.5f * i * triangleHeight;

                // Calculate the position for the triangle
                float xOffset = j * triangleWidth * 0.5f;
                float yOffset = i * triangleHeight * 1f; // Adjust for alternating rows
                Vector3 position = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z + 0);

                // Instantiate the triangle prefab at the calculated position
                GameObject triangle;
                if ((i + j) % 2 == 1)
                {
                    triangle = Instantiate(trianglePrefab, position, Quaternion.identity, transform);

                }
                else
                {
                    triangle = Instantiate(upsideDownTrianglePrefab, position, Quaternion.identity, transform);

                }

                // Set the triangle thickness
                TriangleMesh triangleMesh = triangle.GetComponent<TriangleMesh>();
                triangleMesh.thickness = triangleThickness;

            }
        }
    }
}
