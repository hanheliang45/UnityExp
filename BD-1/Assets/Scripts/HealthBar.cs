using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] HealthSystem hs;

    Image bar;

    void Start()
    {
        hs.OnDamaged += Hs_OnDamaged;
        bar = this.transform.Find("Bar").GetComponent<Image>();

        this.gameObject.SetActive(false);
    }

    private void Hs_OnDamaged(object sender, System.EventArgs e)
    {
        this.gameObject.SetActive(true);
        bar.fillAmount = hs.GetHPNormalized();
    }

}
