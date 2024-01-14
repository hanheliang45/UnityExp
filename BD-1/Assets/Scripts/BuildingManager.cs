using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] BuildingTypeSO buildingType;

    private BuildingTypeListSO btList;

    private void Awake()
    {
        btList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        Debug.Log(btList);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(btList.buildingTypeList[0].prefab, GetMousePosition(), Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Instantiate(btList.buildingTypeList[1].prefab, GetMousePosition(), Quaternion.identity);
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        return mousePosition;
    }
}
