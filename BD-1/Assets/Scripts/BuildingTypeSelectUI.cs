using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    Transform btnTemplate;

    Dictionary<BuildingTypeSO, Transform> buttonDictionary;

    [SerializeField] Sprite arrowSprite;

    Transform arrowBtn;

    void Awake()
    {
        btnTemplate = transform.Find("BtnTemplate");
        btnTemplate.gameObject.SetActive(false);

        buttonDictionary = new Dictionary<BuildingTypeSO, Transform>();
    }

    void Start()
    {
        BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        arrowBtn = Instantiate(btnTemplate, this.transform);
        arrowBtn.Find("Image").GetComponent<Image>().sprite = arrowSprite;
        arrowBtn.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2 (50, 50);
        arrowBtn.GetComponent<Button>().onClick.AddListener(() => {
            BuildingManager.Instance.SetSelectedBuildingType(null);
        });
        arrowBtn.gameObject.SetActive(true);

        int index = 0;
        foreach (BuildingTypeSO buildingType in buildingTypeList.buildingTypeList)
        {
            Transform btn = Instantiate(btnTemplate, this.transform);
            btn.Find("Image").GetComponent<Image>().sprite = buildingType.renderSprite;
            btn.GetComponent<RectTransform>().anchoredPosition = new Vector2((index +1) * 130, 0);
            btn.GetComponent<Button>().onClick.AddListener(() => { 
                BuildingManager.Instance.SetSelectedBuildingType(buildingType);
            });
            btn.gameObject.SetActive(true);

            buttonDictionary[buildingType] = btn;

            index++;
        }

        BuildingManager.Instance.OnSelectBuildingType += BuildingManager_OnSelectBuildingType;
    }

    private void BuildingManager_OnSelectBuildingType(object sender, System.EventArgs e)
    {
        UpdateButton();
    }

    void UpdateButton()
    {
        BuildingTypeSO btSO = BuildingManager.Instance.GetSelectedBuildingType();
        arrowBtn.Find("Selected").gameObject.SetActive(btSO == null);
        foreach (BuildingTypeSO buildingType in buttonDictionary.Keys)
        {
            buttonDictionary[buildingType].Find("Selected").gameObject.SetActive(
                btSO == buildingType    
            );
        }
        
    }
}
