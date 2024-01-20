using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceNearby : MonoBehaviour
{
    SpriteRenderer ICON;
    TextMeshPro text;

    void Start()
    {
        Hide();

        ICON = transform.Find("ICON").GetComponent<SpriteRenderer>();
        text = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    public void Show(ResourceGeneratorData rgd, int nearby)
    { 
        gameObject.SetActive(true);

        ICON.sprite = rgd.resourceType.sprite;
        text.text = Mathf.FloorToInt(((float)nearby / rgd.maxValidResourceAmount) * 100).ToString() + "%";
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
