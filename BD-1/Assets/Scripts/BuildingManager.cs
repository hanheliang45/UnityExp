using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    public event EventHandler OnSelectBuildingType;

    private BuildingTypeSO selectedBuildingType;

    private void Awake()
    {
        Instance = this;

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
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
        Vector3 mousePosition = Tools.GetMouseWorldPosition();
        if (!CanBuild(selectedBuildingType, mousePosition))
        {
            return;
        }
        if (!ResourceManager.Instance.CanAfford(selectedBuildingType))
        {
            return;
        }

        ResourceManager.Instance.SpendResource(selectedBuildingType);
        Instantiate(selectedBuildingType.prefab, mousePosition, Quaternion.identity);
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

    private bool CanBuild(BuildingTypeSO buildingType, Vector3 position)
    {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            (Vector2)position + boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = colliders.Length == 0;
        if (!isAreaClear)
        {
            return false;
        }

        float minRadius = 8f;
        colliders = Physics2D.OverlapCircleAll(position, minRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<BuildingTypeHolder>(out BuildingTypeHolder holder))
            {
                if (holder.GetBuildingTypeSO() == buildingType)
                {
                    return false;
                }
            }
        }

        float maxRadius = 40f;
        colliders = Physics2D.OverlapCircleAll(position, maxRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<BuildingTypeHolder>(out BuildingTypeHolder holder))
            {
                return true;
            }
        }

        return false;
    }
}
