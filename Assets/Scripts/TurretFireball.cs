using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFireball : MonoBehaviour
{
    public float speed = 10f;
    public float destructionTime = 3f;
    private Rigidbody2D rb;

    private Vector2 velocity;
    private float damage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(destructionTime);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = velocity;
    }

    public void SetDirection(Vector2 direction)
    {
        velocity = direction * speed;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.parent.tag == "Monster")
        {
            MonsterLogic monster = collision.GetComponentInParent<MonsterLogic>();
            monster.Damage(damage);
            Destroy(gameObject);
        }
    }
}
