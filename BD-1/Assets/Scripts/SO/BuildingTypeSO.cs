using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public Sprite renderSprite;
    public int HP;
    public float sameTypeDistance = 8f;

    public ResourceGeneratorData rgd;
    public ResourceAmount[] rcArray;

    public string GetResourceCostString()
    {
        string str = "";
        foreach (ResourceAmount c in rcArray)
        {
            str += "<color=#" + c.resourceType.colorHex + ">" 
                + c.resourceType.shortName + ":" + c.amount + "</color> ";
        }
        return str;
    }
}
