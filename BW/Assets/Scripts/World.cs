using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public static readonly int WORLD_SIZE_CHUNK = 30;

    public static readonly int SEED = 12345678;

    [SerializeField] Transform player;
    private Vector3 playerSpawnPosition;

    private float refreshTimer = 0;
    private float refreshTimerMax = 0.5f;

    public BiomeAttributes biomeAttributes;

    public Material material;

    public BlockType[] blockTypes;

    Chunk[,] chunks = new Chunk[WORLD_SIZE_CHUNK, WORLD_SIZE_CHUNK];

    List<ChunkCoord> activeChunks = new List<ChunkCoord>();

    ChunkCoord playerLastCC;

    void Start()
    {
        UnityEngine.Random.InitState(SEED);

        playerSpawnPosition = new Vector3(
            WORLD_SIZE_CHUNK * Chunk.CHUNK_WIDTH / 2, 2,
            WORLD_SIZE_CHUNK * Chunk.CHUNK_WIDTH / 2);
        playerLastCC = GetChunkCoordFromVector3(playerSpawnPosition);

        GenerateWorld();

        
        player.position = playerSpawnPosition;
    }


    private void Update()
    {
        refreshTimer += Time.deltaTime;
        if (refreshTimer >= refreshTimerMax 
            && !playerLastCC.Equals(GetChunkCoordFromVector3(player.position)))
        {
            refreshTimer = 0;
            CheckViewDistance();
        }

        playerLastCC = GetChunkCoordFromVector3(player.position);

    }

    void GenerateWorld()
    {
        for (int x = WORLD_SIZE_CHUNK/2 - VoxelData.VIEW_DISTANCE_IN_CHUNKS;
                x <= WORLD_SIZE_CHUNK / 2 + VoxelData.VIEW_DISTANCE_IN_CHUNKS; x++)
        {
            for (int z = WORLD_SIZE_CHUNK / 2 - VoxelData.VIEW_DISTANCE_IN_CHUNKS;
                    z <= WORLD_SIZE_CHUNK / 2 + VoxelData.VIEW_DISTANCE_IN_CHUNKS; z++)
            {
                CreateNewChunk(x, z);
            }
        }
    }

    ChunkCoord GetChunkCoordFromVector3(Vector3 pos)
    {
        return new ChunkCoord(
            Mathf.FloorToInt(pos.x / Chunk.CHUNK_WIDTH),
            Mathf.FloorToInt(pos.z / Chunk.CHUNK_WIDTH)
            );
    }

    void CheckViewDistance()
    {
        ChunkCoord middle = GetChunkCoordFromVector3(player.transform.position);

        List<ChunkCoord> previousActiveChunks = new List<ChunkCoord>(activeChunks);

        for (int x = middle.x - VoxelData.VIEW_DISTANCE_IN_CHUNKS;
                x <= middle.x + VoxelData.VIEW_DISTANCE_IN_CHUNKS; x++)
        {
            for (int z = middle.z - VoxelData.VIEW_DISTANCE_IN_CHUNKS;
                z <= middle.z + VoxelData.VIEW_DISTANCE_IN_CHUNKS; z++)
            {
                ChunkCoord cc = new ChunkCoord(x, z);
                if (isChunkInWorld(new ChunkCoord(x, z)))
                {
                    if (chunks[x, z] == null)
                    {
                        CreateNewChunk(x, z);
                    }
                    else if (!chunks[x, z].isActive())
                    {
                        chunks[x, z].SetActive(true);
                        activeChunks.Add(cc);
                    }
                    
                }

                for (int i = 0; i < previousActiveChunks.Count; i++)
                {
                    if (previousActiveChunks[i].Equals(cc))
                    {
                        previousActiveChunks.RemoveAt(i);
                    }
                }
            }
        }

        foreach (ChunkCoord cc in previousActiveChunks)
        {
            chunks[cc.x, cc.z].SetActive(false);
            activeChunks.Remove(cc);
        }

    }

    public Chunk GetChunk(int x, int z)
    {
        if (x <0 || x >= WORLD_SIZE_CHUNK || z < 0 || z >= WORLD_SIZE_CHUNK)
        {
            return null;
        }
        return chunks[x, z];
    }

    void CreateNewChunk(int x, int z)
    {
        ChunkCoord cc = new ChunkCoord(x, z);
        chunks[x, z] = new Chunk(this, cc);

        activeChunks.Add(cc);
    }

    public byte GetBlockType(Vector3 voxelPosition)
    {
        if (!isVoxelInWorld(voxelPosition))
        {
            return 0;
        }

        int yPos = Mathf.FloorToInt(voxelPosition.y);
        if (yPos == 0)
        {
            return 1;
        }

        byte voxelValue = 0;

        int terrainHeight = (int)(biomeAttributes.terrainHeight
            * Noise.Get2DPerlin(voxelPosition.x, voxelPosition.z,
            biomeAttributes.terrainScale,
            biomeAttributes.terrainOffset) + biomeAttributes.solidGroundHeight);
        if (yPos == terrainHeight)
        {
            voxelValue = 3;
        }
        else if (yPos < terrainHeight)
        {
            if (yPos > terrainHeight - 4)
            {
                voxelValue = 5;
            }
            else
            {
                voxelValue = 2;
            }
        }
        else
        {
            return 0;
        }

        if (voxelValue == 2)
        { 
            foreach (Lode lode in biomeAttributes.lodes)
            {
                if (yPos < lode.minHeight || yPos > lode.maxHeight)
                {
                    continue;
                }
                if (Noise.Get3DPerlin(voxelPosition.x, voxelPosition.y, voxelPosition.z,
                    lode.scale, lode.noiseOffset, lode.threshold))
                {
                    return lode.blockID;
                }
            }
        }

        return voxelValue;
    }

    bool isChunkInWorld(ChunkCoord cc)
    {
        if (cc.x < 0 || cc.x >= WORLD_SIZE_CHUNK)
        {
            return false;
        }
        if (cc.z < 0 || cc.z >= WORLD_SIZE_CHUNK)
        {
            return false;
        }
        return true;
    }

    bool isVoxelInWorld(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);
        int z = Mathf.FloorToInt(position.z);

        if (position.x < 0 || position.x >= Chunk.CHUNK_WIDTH * World.WORLD_SIZE_CHUNK)
        {
            return false;
        }
        if (position.y < 0 || position.y >= Chunk.CHUNK_HEIGHT)
        {
            return false;
        }
        if (position.z < 0 || position.z >= Chunk.CHUNK_WIDTH * World.WORLD_SIZE_CHUNK)
        {
            return false;
        }

        return true;
    }
}


[Serializable]
public class BlockType
{
    public string blockName;
    public bool isSolid;

    [Header("Texture Values")]
    public int backFaceTexture;
    public int rightFaceTexture;
    public int forwardFaceTexture;
    public int leftFaceTexture;
    public int topFaceTexture;
    public int bottomFaceTexture;

    // back, right, forward, left, top, bottom
    public int GetTextureID(int faceIndex)
    {
        switch (faceIndex)
        {
            case 0:
                return backFaceTexture;
            case 1:
                return rightFaceTexture;
            case 2:
                return forwardFaceTexture;
            case 3:
                return leftFaceTexture;
            case 4:
                return topFaceTexture;
            case 5:
                return bottomFaceTexture;
        }
        Debug.LogError("wrong faceIndex");
        return backFaceTexture;
    }
}


