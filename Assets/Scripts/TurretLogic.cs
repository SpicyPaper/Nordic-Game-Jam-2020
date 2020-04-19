using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLogic : MonoBehaviour
{
    public float TurretDamage = 20;
    public float attackSpeed = 1f;
    public GameObject projectilePrefab;

    private bool canAttack = true;

    private List<GameObject> enemies = new List<GameObject>();

    private void Update()
    {
        if (canAttack && enemies.Count > 0)
        {
            enemies.RemoveAll(item => item == null);

            enemies.Sort((a, b) => (Vector3.Distance(a.transform.position, transform.position).CompareTo(Vector3.Distance(b.transform.position, transform.position))));

            Shoot(enemies[0]);

            StartCoroutine(WaitForCooldown());
        }
    }

    private void Shoot(GameObject enemy)
    {
        if (enemy != null)
        {
            var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<TurretFireball>();
            var direction = enemy.transform.position - transform.position;
            
            projectile.SetDirection(new Vector2(direction.x, direction.y).normalized);
            projectile.SetDamage(TurretDamage);
        }
    }

    IEnumerator WaitForCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.tag == "Monster")
        {
            enemies.Add(collision.transform.parent.gameObject);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.tag == "Monster")
        {
            enemies.Remove(collision.transform.parent.gameObject);
        }
    }

}
