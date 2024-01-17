using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionSorting : MonoBehaviour
{
    [SerializeField] bool runOnce;
    [SerializeField] float offset;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        float precisionMultiplier = 10f;
        sr.sortingOrder = -(int)((transform.position.y + offset) * precisionMultiplier);
        if (runOnce)
        {
            Destroy(this);
        }
    }
}
