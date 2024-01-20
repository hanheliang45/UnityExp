using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceGeneratorProgress : MonoBehaviour
{
    [SerializeField] ResourceGenerator rg;

    private Transform bar;

    void Start()
    {
        BuildingTypeSO buildingType = rg.GetBuildingType();

        transform.Find("ICON").GetComponent<SpriteRenderer>().sprite
            = buildingType.rgd.resourceType.sprite;
        transform.Find("Text").GetComponent<TextMeshPro>().text = 
            (1 / rg.GetMaxTime()).ToString("F1");

        bar = transform.Find("Bar");
    }

    // Update is called once per frame
    void Update()
    {
        bar.transform.localScale = new Vector3(rg.GetGenerationNormalized(), 1, 1);
    }
}
