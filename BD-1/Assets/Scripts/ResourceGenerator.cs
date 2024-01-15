using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    float timer;
    float timerMax;
    BuildingTypeSO buildingType;
       
    void Start()
    {
        this.buildingType = GetComponent<BuildingTypeHolder>().GetBuildingTypeSO();
        this.timerMax = buildingType.rgd.timerMax;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timerMax)
        {
            timer = 0;

            Debug.Log("Generating resource " + this.buildingType.nameString);

            ResourceManager.Instance.AddResource(buildingType.rgd.resourceType, 1);
        }
    }
}
