using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUI : MonoBehaviour
{
    Dictionary<ResourceTypeSO, Transform> resourceElementMap;
    
    void Awake()
    {
        resourceElementMap = new Dictionary<ResourceTypeSO, Transform>();

        Transform resourceTemplate = transform.Find("ResourceTemplate");
        resourceTemplate.gameObject.SetActive(false);

        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        int index = 0;
        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            Transform resourceElement = Instantiate(resourceTemplate, this.transform);

            resourceElement.GetComponent<RectTransform>().anchoredPosition = new Vector2(index * -160, 0);
            resourceElement.Find("Image").GetComponent<Image>().sprite = resourceType.sprite;
            resourceElement.Find("Text").GetComponent<TextMeshProUGUI>().text = "0";
            resourceElement.gameObject.SetActive(true);

            resourceElementMap.Add(resourceType, resourceElement);

            index++;
        }
    }

    private void Start()
    {
        ResourceManager.Instance.OnAddResource += ResourceManager_OnAddResource;
    }

    private void ResourceManager_OnAddResource(object sender, ResourceTypeSO e)
    {
        UpdateResourceAmount(e);
    }
    void UpdateResourceAmount(ResourceTypeSO resourceType)
    {
        int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);
        Debug.Log("Resource Amount: " + resourceAmount);
        resourceElementMap[resourceType].Find("Text").GetComponent<TextMeshProUGUI>().text
                   = resourceAmount.ToString();
    }

}
