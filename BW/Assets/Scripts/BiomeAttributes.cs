using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BiomeAttributes")]
public class BiomeAttributes : ScriptableObject
{
    public string biomeName;

    public int solidGroundHeight;

    public int terrainHeight;
    public float terrainScale;
    public float terrainOffset;

    public Lode[] lodes;
}

[Serializable]
public class Lode
{ 
    public string lodeName;
    public byte blockID;
    public int minHeight;
    public int maxHeight;

    public float scale;
    public float threshold;
    public float noiseOffset;
}