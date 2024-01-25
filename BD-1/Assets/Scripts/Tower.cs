using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] Transform arrowPoint;
    private Enemy target;
    private float aimingTimer;

    private float shootTimerMax = 0.1f;
    private float shootTime;
    void Start()
    {
        aimingTimer = 0;
        shootTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        LookForTarget();
        Shoot();
    }

    void Shoot()
    {
        if (this.target == null)
        {
            return;
        }

        shootTime += Time.deltaTime;
        if (shootTime >= shootTimerMax)
        {
            Arrow.CreateArrow(this.arrowPoint.position, target);
            shootTime = 0;
        }
    }

    void LookForTarget()
    {
        aimingTimer += Time.deltaTime;
        if (aimingTimer <= .5f)
        {
            return;
        }
        aimingTimer = 0;
        foreach (Collider2D collider in
            Physics2D.OverlapCircleAll(this.transform.position, 20f))
        {
            if (collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                if (this.target == null)
                {
                    this.target = enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, target.transform.position)
                        > Vector3.Distance(transform.position, enemy.transform.position))
                    {
                        this.target = enemy;
                    }
                }
                return;
            }

        }
    }
}
