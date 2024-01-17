using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BuildingGhost : MonoBehaviour
{
    private GameObject sprite;
    private bool isShow;

    void Start()
    {
        sprite = this.transform.Find("Sprite").gameObject;

        Hide();

        BuildingManager.Instance.OnSelectBuildingType += BuildingManager_OnSelectBuildingType;
    }

    private void BuildingManager_OnSelectBuildingType(object sender, System.EventArgs e)
    {
        BuildingTypeSO buildingType = BuildingManager.Instance.GetSelectedBuildingType();
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
