using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChunkCoord
{
    public int x;
    public int z;

    public ChunkCoord(int x, int z)
    {
        this.x = x; this.z = z;
    }

    public bool Equals(ChunkCoord other)
    {
        if (other == null)
        {
            return false;
        }
        return this.x == other.x && this.z == other.z;
    }
}
public class Chunk 
{
    public static readonly int CHUNK_WIDTH = 5;
    public static readonly int CHUNK_HEIGHT = 120;

    ChunkCoord cood;

    GameObject chunkGO;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    World world;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();
    int vertIndex = 0;

    public byte[,,] voxelMap = new byte[
        CHUNK_WIDTH,
        CHUNK_HEIGHT,
        CHUNK_WIDTH];

    public Chunk(World _world, ChunkCoord cood)
    {
        this.world = _world;
        this.cood = cood;

        this.chunkGO = new GameObject("Chunk_"+cood.x+"_"+cood.z);
        this.meshFilter = chunkGO.AddComponent<MeshFilter>();
        this.meshRenderer = chunkGO.AddComponent<MeshRenderer>();
        this.meshRenderer.material = world.material;

        this.chunkGO.transform.parent = this.world.transform;
        this.chunkGO.transform.position = new Vector3(cood.x * CHUNK_WIDTH, 0f, cood.z * CHUNK_WIDTH);

        populateVoxelMap();

        AddVoxelData();

        CreateMesh();
    }

    void AddSingleVoxelData(Vector3 position)
    {
        if (this.voxelMap[(int)position.x, (int)position.y,  (int)position.z] == 0)
        {
            return;
        }
        for (int faceIndex = 0; faceIndex < 6; faceIndex++)
        {
            Vector3 adjacent = position + VoxelData.FACE_CHECKS[faceIndex];
            if (hasVoxel(adjacent))
            {
                continue;
            }
            vertices.Add(position + 
                VoxelData.VOXEL_VERTS[VoxelData.VOXEL_TRIANGLES[faceIndex, 0]]);
            vertices.Add(position +
               VoxelData.VOXEL_VERTS[VoxelData.VOXEL_TRIANGLES[faceIndex, 1]]);
            vertices.Add(position +
               VoxelData.VOXEL_VERTS[VoxelData.VOXEL_TRIANGLES[faceIndex, 2]]);
            vertices.Add(position +
               VoxelData.VOXEL_VERTS[VoxelData.VOXEL_TRIANGLES[faceIndex, 3]]);

            int textureID =
                world.blockTypes[
                voxelMap[
                    Mathf.FloorToInt(position.x),
                    Mathf.FloorToInt(position.y),
                    Mathf.FloorToInt(position.z)
                ]].GetTextureID(faceIndex);

            AddTexture(textureID);

            triangles.Add(vertIndex);
            triangles.Add(vertIndex+1);
            triangles.Add(vertIndex+2);
            triangles.Add(vertIndex+2);
            triangles.Add(vertIndex+1);
            triangles.Add(vertIndex+3);

            vertIndex += 4;
        }
    }

    void AddTexture(int textureId)
    {
        int row = 3 -  textureId / VoxelData.TEXTURE_SIZE_IN_BLOCKS;
        int column = textureId % VoxelData.TEXTURE_SIZE_IN_BLOCKS;
        Vector2 uv_0 = new Vector2(
            column * VoxelData.TEXTURE_SIZE_NORMALIZED,
            row * VoxelData.TEXTURE_SIZE_NORMALIZED
        );
        Vector2 uv_1 = new Vector2(
            column * VoxelData.TEXTURE_SIZE_NORMALIZED,
            row * VoxelData.TEXTURE_SIZE_NORMALIZED + VoxelData.TEXTURE_SIZE_NORMALIZED
        );
        Vector2 uv_2 = new Vector2(
            column * VoxelData.TEXTURE_SIZE_NORMALIZED + VoxelData.TEXTURE_SIZE_NORMALIZED,
            row * VoxelData.TEXTURE_SIZE_NORMALIZED
        );
        Vector2 uv_3 = new Vector2(
            column * VoxelData.TEXTURE_SIZE_NORMALIZED + VoxelData.TEXTURE_SIZE_NORMALIZED,
            row * VoxelData.TEXTURE_SIZE_NORMALIZED + VoxelData.TEXTURE_SIZE_NORMALIZED
        );
        uvs.Add(uv_0);
        uvs.Add(uv_1);
        uvs.Add(uv_2);
        uvs.Add(uv_3);
    }


    public bool isActive()
    {
        return this.chunkGO.activeSelf;
    }
    public void SetActive(bool value)
    {
        this.chunkGO.SetActive(value);
    }
    public Vector3 GetPosition()
    {
        return this.chunkGO.transform.position;
    }

    public static bool isVoxelInChunk(int x, int y, int z)
    {
        if (x < 0 || x >= CHUNK_WIDTH)
        {
            return false;
        }
        if (y < 0 || y >= CHUNK_HEIGHT)
        {
            return false;
        }
        if (z < 0 || z >= CHUNK_WIDTH)
        {
            return false;
        }
        return true;
    }

    bool hasVoxel(Vector3 position)
    {
        Vector3 positionInWorld = GetPosition() + position;

        int x = Mathf.FloorToInt(positionInWorld.x);
        int y = Mathf.FloorToInt(positionInWorld.y);
        int z = Mathf.FloorToInt(positionInWorld.z);

        byte blockType = 0;
        if (isVoxelInChunk(x, y, z))
        {
            blockType = this.voxelMap[x, y, z];
        }
        else
        {
            blockType = this.world.GetBlockType(positionInWorld);
        }
        return world.blockTypes[blockType].isSolid;
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
        for (int i = 0; i < CHUNK_WIDTH; i++)
        {
            for (int j = 0; j < CHUNK_HEIGHT; j++)
            {
                for (int k = 0; k < CHUNK_WIDTH; k++)
                {
                    voxelMap[i, j, k] = world.GetBlockType(new Vector3(i, j, k) + 
                        GetPosition());
                }
            }
        }
    }

    void AddVoxelData()
    {
        for (int i = 0; i < CHUNK_WIDTH; i++)
        {
            for (int j = 0; j < CHUNK_HEIGHT; j++)
            {
                for (int k = 0; k < CHUNK_WIDTH; k++)
                {
                    AddSingleVoxelData(new Vector3(i, j, k));
                }
            }
        }
    }

}
