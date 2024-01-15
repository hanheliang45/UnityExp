using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public event EventHandler<ResourceTypeSO> OnAddResource;

    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;

    void Awake()
    {
        Instance = this;

        resourceAmountDictionary  = new Dictionary<ResourceTypeSO, int>();

        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        foreach (ResourceTypeSO resource in resourceTypeList.list)
        {
            resourceAmountDictionary[resource] = 0;
        }

        Debug.Log("resourceAmountDictionary " + resourceAmountDictionary.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
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
