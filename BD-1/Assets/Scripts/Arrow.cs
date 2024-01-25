using UnityEngine;

public class Arrow : MonoBehaviour
{
    public static Arrow CreateArrow(Vector3 position, Enemy target)
    {
        Transform prefab = Resources.Load<Arrow>("Arrow").transform;
        Transform arrowT = Instantiate(prefab, position, Quaternion.identity);
        Arrow arrow = arrowT.GetComponent<Arrow>();
        arrow.SetTarget(target);
        return arrow;
    }
    private Enemy target;
    private Vector3 orginalDirection;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
    }

    public void SetTarget(Enemy target)
    { 
        this.target = target;
    }

    void HandleMove()
    {
        Vector3 direction;
        if (target == null)
        {
            direction = this.orginalDirection;
            Destroy(this.gameObject, 2f);
        }
        else
        {
            direction = (target.transform.position - this.transform.position)
                    .normalized;
            this.orginalDirection = direction;
        }

        float speed = 30f;
        this.transform.position += direction * Time.deltaTime * speed;

        this.transform.eulerAngles = new Vector3(0, 0,
                Tools.Direction2Degree(direction));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            Destroy(this.gameObject);

            enemy.GetComponent<HealthSystem>().TakeDamage(50);
        }
    }
}
