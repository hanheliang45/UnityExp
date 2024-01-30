using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelData
{
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

    public static readonly int[,] VOXEL_TRIANGLES = new int[6,6] {
        { 0,3,1,1,3,2}, // back
        { 1,2,5,5,2,6}, // right
        { 5,6,4,4,6,7}, // forward
        { 4,7,0,0,7,3}, // left
        { 3,7,2,2,7,6}, // top
        { 0,1,4,4,1,5}  // bottom
    };

    public static readonly Vector2[] VOXEL_UVS = new Vector2[6] {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
    };


}
