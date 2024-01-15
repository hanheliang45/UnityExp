using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

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
            Instantiate(selectedBuildingType.prefab, GetMousePosition(), Quaternion.identity);
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        return mousePosition;
    }

    public void SetSelectedBuildingType(BuildingTypeSO buildingType)
    { 
        this.selectedBuildingType = buildingType;
    }

    public BuildingTypeSO GetSelectedBuildingType()
    {
        return this.selectedBuildingType;
    }
}
