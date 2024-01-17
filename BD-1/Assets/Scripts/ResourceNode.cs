using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField] ResourceTypeSO resourceType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public ResourceTypeSO getResourceType() { return resourceType; }
}
