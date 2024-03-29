using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelData
{

    public static readonly int VIEW_DISTANCE_IN_CHUNKS = 10;

    public static readonly int TEXTURE_SIZE_IN_BLOCKS = 4;
    public static readonly float TEXTURE_SIZE_NORMALIZED = 1f / TEXTURE_SIZE_IN_BLOCKS;

    public static readonly Vector3[] VOXEL_VERTS = new Vector3[8] { 
        new Vector3 (0, 0, 0),
        new Vector3 (1, 0, 0),
        new Vector3 (1, 1, 0),
        new Vector3 (0, 1, 0),
        new Vector3 (0, 0, 1),
        new Vector3 (1, 0, 1),
        new Vector3 (1, 1, 1),
        new Vector3 (0, 1, 1),
    };

    public static readonly int[,] VOXEL_TRIANGLES = new int[6,4] {
        // back, right, forward, left, top, bottom
        { 0,3,1,2}, // back
        { 1,2,5,6}, // right
        { 5,6,4,7}, // forward
        { 4,7,0,3}, // left
        { 3,7,2,6}, // top
        { 0,1,4,5}  // bottom
    };

    //public static readonly Vector2[] VOXEL_UVS = new Vector2[4] {
    //        new Vector2(0, 0),
    //        new Vector2(0, 1),
    //        new Vector2(1, 0),
    //        new Vector2(1, 1)
    //};

    public static readonly Vector3[] FACE_CHECKS = new Vector3[6]{
        new Vector3(0, 0, -1),
        new Vector3(1, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(-1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(0, -1, 0)
    };


}
