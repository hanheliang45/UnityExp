using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private List<ResourceAmount> startResourceAmounts;

    public static ResourceManager Instance;

    public event EventHandler<ResourceTypeSO> OnAddResource;

    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;

    void Awake()
    {
        Instance = this;
        
        resourceAmountDictionary  = new Dictionary<ResourceTypeSO, int>();

        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        
        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            resourceAmountDictionary[resourceType] = 0;
        }
    }

    private void Start()
    {
        foreach (ResourceAmount rc in startResourceAmounts)
        {
            AddResource(rc.resourceType, rc.amount);
        }
    }

    public bool CanAfford(BuildingTypeSO buildingType)
    {
        foreach (ResourceAmount rc in buildingType.rcArray)
        {
            if (resourceAmountDictionary[rc.resourceType] < rc.amount)
            {
                return false;
            }
        }
        return true;
    }

    public void SpendResource(BuildingTypeSO buildingType)
    {
        foreach (ResourceAmount rc in buildingType.rcArray)
        {
            resourceAmountDictionary[rc.resourceType] -= rc.amount;
            OnAddResource?.Invoke(this, rc.resourceType);
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        resourceAmountDictionary[resourceType] += amount;

        OnAddResource?.Invoke(this, resourceType);
    }

    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return resourceAmountDictionary[resourceType];
    }
}
