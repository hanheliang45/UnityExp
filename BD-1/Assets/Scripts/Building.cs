using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    HealthSystem hs;

    void Start()
    {
        this.hs = GetComponent<HealthSystem>();
        this.hs.SetMaxHP(
            GetComponent<BuildingTypeHolder>().GetBuildingTypeSO().HP
            ) ;

        this.hs.OnDied += Hs_OnDied;
        this.hs.OnDamaged += Hs_OnDamaged;
    }

    void Update()
    {

    }

    private void Hs_OnDamaged(object sender, System.EventArgs e)
    {
        
    }

    private void Hs_OnDied(object sender, System.EventArgs e)
    {
        Destroy(this.gameObject);
    }

}
