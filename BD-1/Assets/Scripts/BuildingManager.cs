using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    public event EventHandler OnSelectBuildingType;

    private BuildingTypeListSO btList;

    private BuildingTypeSO selectedBuildingType;
    private void Awake()
    {
        Instance = this;

        btList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (selectedBuildingType == null)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(selectedBuildingType.prefab, Tools.GetMouseWorldPosition(), Quaternion.identity);
        }
    }


    public void SetSelectedBuildingType(BuildingTypeSO buildingType)
    { 
        this.selectedBuildingType = buildingType;

        this.OnSelectBuildingType?.Invoke(this, null);
    }

    public BuildingTypeSO GetSelectedBuildingType()
    {
        return this.selectedBuildingType;
    }
}
