using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();
    int vertIndex = 0;

    bool[,,] voxelMap = new bool[
        VoxelData.CHUNK_WIDTH,
        VoxelData.CHUNK_WIDTH,
        VoxelData.CHUNK_HEIGHT];

    void AddSingleVoxelData(Vector3 position)
    {
        for (int faceIndex = 0; faceIndex < 6; faceIndex++)
        {
            Vector3 adjacent = position + VoxelData.FACE_CHECKS[faceIndex];
            if (hasVoxel(adjacent))
            {
                continue;
            }
            vertices.Add(position + 
                VoxelData.VOXEL_VERTS[VoxelData.VOXEL_TRIANGLES[faceIndex, 0]]);
            uvs.Add(VoxelData.VOXEL_UVS[0]);
            vertices.Add(position +
               VoxelData.VOXEL_VERTS[VoxelData.VOXEL_TRIANGLES[faceIndex, 1]]);
            uvs.Add(VoxelData.VOXEL_UVS[1]);
            vertices.Add(position +
               VoxelData.VOXEL_VERTS[VoxelData.VOXEL_TRIANGLES[faceIndex, 2]]);
            uvs.Add(VoxelData.VOXEL_UVS[2]);
            vertices.Add(position +
               VoxelData.VOXEL_VERTS[VoxelData.VOXEL_TRIANGLES[faceIndex, 3]]);
            uvs.Add(VoxelData.VOXEL_UVS[3]);

            triangles.Add(vertIndex);
            triangles.Add(vertIndex+1);
            triangles.Add(vertIndex+2);
            triangles.Add(vertIndex+2);
            triangles.Add(vertIndex+1);
            triangles.Add(vertIndex+3);

            vertIndex += 4;
        }
    }

    bool hasVoxel(Vector3 position)
    {
        if (position.x < 0 || position.x >= VoxelData.CHUNK_WIDTH)
        {
            return false;
        }
        if (position.y < 0 || position.y >= VoxelData.CHUNK_WIDTH)
        {
            return false;
        }
        if (position.z < 0 || position.z >= VoxelData.CHUNK_HEIGHT)
        {
            return false;
        }
        return voxelMap[
            Mathf.FloorToInt(position.x),
            Mathf.FloorToInt(position.y),
            Mathf.FloorToInt(position.z)
            ];
    }

    public void CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    void populateVoxelMap()
    {
        for (int i = 0; i < VoxelData.CHUNK_HEIGHT; i++)
        {
            for (int j = 0; j < VoxelData.CHUNK_WIDTH; j++)
            {
                for (int k = 0; k < VoxelData.CHUNK_WIDTH; k++)
                {
                    voxelMap[i, j, k] = true;
                }
            }
        }
    }

    void AddVoxelData()
    {
        for (int i = 0; i < VoxelData.CHUNK_HEIGHT; i++)
        {
            for (int j = 0; j < VoxelData.CHUNK_WIDTH; j++)
            {
                for (int k = 0; k < VoxelData.CHUNK_WIDTH; k++)
                {
                    AddSingleVoxelData(new Vector3(i, j, k));
                }
            }
        }
    }

    void Start()
    {
        populateVoxelMap();

        AddVoxelData();

        CreateMesh();
    }

}
