using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    [SerializeField] private Building hqBuilding;

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
        if (!CanBuild(selectedBuildingType, mousePosition, out string errorMsg))
        {
            ToolTipUI.Instance.Show(errorMsg, new ToolTipUI.TooltipTimer { timer = 2f });
            return;
        }
        if (!ResourceManager.Instance.CanAfford(selectedBuildingType))
        {
            ToolTipUI.Instance.Show("you can not affort!",
                new ToolTipUI.TooltipTimer { timer = 2f});
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

    private bool CanBuild(BuildingTypeSO buildingType, Vector3 position,
                        out string errorMessage)
    {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            (Vector2)position + boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = colliders.Length == 0;
        if (!isAreaClear)
        {
            errorMessage = "overlap with other object";
            return false;
        }

        colliders = Physics2D.OverlapCircleAll(position, buildingType.sameTypeDistance);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<BuildingTypeHolder>(out BuildingTypeHolder holder))
            {
                if (holder.GetBuildingTypeSO() == buildingType)
                {
                    errorMessage = "too close buidling with same type";
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
                errorMessage = "";
                return true;
            }
        }

        errorMessage = "too far away with other buildings";
        return false;
    }

    public Building GetHQBuilding()
    {
        return this.hqBuilding;
    }
}
