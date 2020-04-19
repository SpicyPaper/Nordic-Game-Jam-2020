using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 2f;
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

    // Update is called once per frame
    void Update()
    {
        rb.velocity = velocity;
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(destructionTime);
        Destroy(gameObject);
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
        if (collision.transform.parent == null)
        {
            return;
        }

        if (collision.transform.parent.tag == "Monster")
        {
            MonsterLogic monster = collision.GetComponentInParent<MonsterLogic>();
            monster.Damage(damage);

            Destroy(gameObject);
        }
    }
}
