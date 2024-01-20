using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    float timer;
    float timerMax;
    BuildingTypeSO buildingType;

    int nearbyResourceAmount;
       
    void Awake()
    {
        this.buildingType = GetComponent<BuildingTypeHolder>().GetBuildingTypeSO();
        this.timerMax = buildingType.rgd.timerMax;
    }

    public static int GetNearbyResourceNodeNumber
        (
        Vector3 position, 
        ResourceGeneratorData rgd
        )
    {
        float resourceDetectionRadius = rgd.resourceDetectionRadius;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, resourceDetectionRadius);

        int resourceNodeNumber = 0;
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<ResourceNode>(out ResourceNode rn))
            {
                if (rn.getResourceType() == rgd.resourceType)
                {
                    resourceNodeNumber++;
                }
            }
        }

        return Mathf.Clamp(resourceNodeNumber, 0, rgd.maxValidResourceAmount);
    }

    private void Start()
    {
        this.nearbyResourceAmount = GetNearbyResourceNodeNumber(this.transform.position, this.buildingType.rgd);

        if (this.nearbyResourceAmount == 0)
        {
            this.enabled = false;
        }
        else
        {
            this.timerMax = this.timerMax / 2 + this.timerMax / 2 * (
                1 - (float)nearbyResourceAmount / this.buildingType.rgd.maxValidResourceAmount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timerMax)
        {
            timer = 0;

            ResourceManager.Instance.AddResource(buildingType.rgd.resourceType, 1);
        }
    }

    public BuildingTypeSO GetBuildingType()
    {
        return this.buildingType;
    }

    public float GetGenerationNormalized()
    {
        return timer / timerMax;
    }

    public float GetMaxTime()
    {
        return this.timerMax;
    }
}
