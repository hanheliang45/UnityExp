using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise 
{

    public static float Get2DPerlin(float x, float z, float scale, float offset)
    { 
        return Mathf.PerlinNoise(
            (x ) * scale + offset,
            (z ) * scale + offset
            );
    }

    public static bool Get3DPerlin(float x, float y, float z, float scale, float offset, float threshold)
    { 
        x = (x + offset) * scale;
        y = (y + offset) * scale;
        z = (z + offset) * scale;

        float XY = Mathf.PerlinNoise(x, y);
        float XZ = Mathf.PerlinNoise(x, z);
        float YZ = Mathf.PerlinNoise(y, z);
        float YX = Mathf.PerlinNoise(y, x);
        float ZX = Mathf.PerlinNoise(z, x);
        float ZY = Mathf.PerlinNoise(z, y);

        return (XY + XZ + YZ + YX + ZX + ZY) / 6 > threshold;
    }

}
