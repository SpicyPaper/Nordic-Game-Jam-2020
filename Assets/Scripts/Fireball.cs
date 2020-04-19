using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 2f;
    private Rigidbody2D rb;

    private Vector2 velocity;
    private float damage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        }

        if (collision.transform.parent.tag != "Player" && collision.transform.parent.tag != "Projectile" && collision.transform.parent.tag != "Turret")
        {
            Destroy(gameObject);
        }
    }
}
