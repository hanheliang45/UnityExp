using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDied;
    public event EventHandler OnDamaged;

    private int maxHP;
    private int healthPoint;

    void Start()
    {
    }

    public void SetMaxHP(int maxHP)
    {
        this.maxHP = maxHP;
        healthPoint = maxHP;
    }

    public void TakeDamage(int damage)
    {
        healthPoint -= damage;
        healthPoint = Mathf.Clamp(healthPoint, 0, maxHP);

        OnDamaged?.Invoke(this, null);
        if (isDead())
        { 
            OnDied?.Invoke(this, null);
        }
    }

    public bool isDead()
    {
        return healthPoint <= 0;
    }

    public int GetHP()
    {
        return healthPoint;
    }

    public float GetHPNormalized()
    {
        return (float)healthPoint / maxHP;
    }

}
