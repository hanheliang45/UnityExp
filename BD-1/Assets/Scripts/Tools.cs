using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{

    private static Camera mainCamera;

    public static Vector3 GetMouseWorldPosition()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        return mousePosition;
    }

    public static Vector3 GetRandomeDirection()
    {
        return new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0).normalized;
    }

    public static float Direction2Degree(Vector3 direction)
    {
        float radian = Mathf.Atan2(direction.y, direction.x);
        return radian * Mathf.Rad2Deg;
    }
}
