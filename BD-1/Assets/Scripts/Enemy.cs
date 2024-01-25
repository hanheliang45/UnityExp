using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create(Vector3 position)
    {
        Transform prefab = Resources.Load<Enemy>("Enemy").transform;
        Transform enemyT = Instantiate(prefab, position, Quaternion.identity);
        return enemyT.GetComponent<Enemy>();
    }

    private Building target;
    private Rigidbody2D body2D;
    private float aimingTimer;
    private HealthSystem hs;

    void Start()
    {
        target = BuildingManager.Instance.GetHQBuilding();
        body2D = GetComponent<Rigidbody2D>();
        hs = GetComponent<HealthSystem>();
        aimingTimer = Random.Range(0, .5f);

        hs.SetMaxHP(100);
        hs.OnDied += Hs_OnDied;
    }

    private void Hs_OnDied(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
        HandleAim();
    }
    void HandleAim()
    {
        aimingTimer += Time.deltaTime;
        if (aimingTimer <= .5f)
        {
            return;
        }
        aimingTimer = 0;
        foreach (Collider2D collider in 
            Physics2D.OverlapCircleAll(this.transform.position, 10f))
        {
            if (collider.gameObject.TryGetComponent<Building>(out Building building))
            {
                if (this.target == null)
                {
                    this.target = building;
                }
                else
                {
                    if (Vector3.Distance(transform.position, target.transform.position)
                        > Vector3.Distance(transform.position, building.transform.position))
                    {
                        this.target = building;
                    }
                }
                return;
            }
            
        }
        target = BuildingManager.Instance.GetHQBuilding();
    }

    void HandleMove()
    {
        if (target == null)
        {
            body2D.velocity = Vector3.zero;
        }
        else
        {
            float speed = 8f;
            body2D.velocity =
                (target.transform.position - this.transform.position).normalized * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Building>(out Building building))
        {
            building.GetComponent<HealthSystem>().TakeDamage(10);

            Destroy(this.gameObject);
        }
    }
}
