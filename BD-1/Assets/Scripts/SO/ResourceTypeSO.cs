using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/ResourceType")]
public class ResourceTypeSO : ScriptableObject
{
    public string nameString;
    public Sprite sprite;
    public string shortName;
    public string colorHex;
}
