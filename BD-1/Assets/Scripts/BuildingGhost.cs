using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BuildingGhost : MonoBehaviour
{
    private GameObject sprite;
    private bool isShow;
    private ResourceNearby resourceNearby;
    private BuildingTypeSO buildingType;

    void Start()
    {
        sprite = this.transform.Find("Sprite").gameObject;
        resourceNearby = this.transform.Find("ResourceNearby").GetComponent<ResourceNearby>();

        Hide();

        BuildingManager.Instance.OnSelectBuildingType += BuildingManager_OnSelectBuildingType;
    }

    private void BuildingManager_OnSelectBuildingType(object sender, System.EventArgs e)
    {
        buildingType = BuildingManager.Instance.GetSelectedBuildingType();
        if (buildingType == null)
        {
            Hide();
        }
        else 
        {
            Show(buildingType.renderSprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isShow)
        {
            this.transform.position = Tools.GetMouseWorldPosition();
            int nodeNumber = ResourceGenerator.GetNearbyResourceNodeNumber(
                this.transform.position, buildingType.rgd);
            if (buildingType.rgd.resourceType != null)
            { 
                resourceNearby.Show(buildingType.rgd, nodeNumber);
            }
        }
    }

    void Show(Sprite ghostSprite)
    {
        sprite.GetComponent<SpriteRenderer>().sprite = ghostSprite;
        sprite.SetActive(true);

        isShow = true;
    }

    void Hide()
    { 
        sprite.SetActive(false);

        isShow=false;
    }
}
