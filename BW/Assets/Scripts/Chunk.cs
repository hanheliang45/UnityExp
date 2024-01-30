using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    List<Vector2> uvs = new List<Vector2>();

    void Start()
    {
        int vertIndex = 0;
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int faceIndex = 0; faceIndex < 6; faceIndex++)
        {
            for (int vertInTriangle = 0; vertInTriangle < 6; vertInTriangle++)
            {
                int vertex = VoxelData.VOXEL_TRIANGLES[faceIndex, vertInTriangle];
                vertices.Add(VoxelData.VOXEL_VERTS[vertex]);

                triangles.Add(vertIndex++);

                uvs.Add(VoxelData.VOXEL_UVS[vertInTriangle]);
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();
        
        meshFilter.mesh = mesh;
        
    }

}
