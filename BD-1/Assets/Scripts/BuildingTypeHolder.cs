using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTypeHolder : MonoBehaviour
{
    [SerializeField] BuildingTypeSO buildingType;

    public BuildingTypeSO GetBuildingTypeSO() { return buildingType; }
}
