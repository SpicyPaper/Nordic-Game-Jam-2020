using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapLogic : MonoBehaviour
{
    public float AttackDamage = 10;
    public float AttackSpeed = 1 / 2f;
    List<MonsterLogic> hitEnemy = new List<MonsterLogic>();

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.parent.tag == "Monster")
        {
            MonsterLogic monster = collision.transform.parent.GetComponent<MonsterLogic>();
            if (!hitEnemy.Contains(monster))
            {
                monster.Damage(AttackDamage);
                StartCoroutine(WaitForDamage(monster));
            }
        }
    }

    IEnumerator WaitForDamage(MonsterLogic monster)
    {
        hitEnemy.Add(monster);
        yield return new WaitForSeconds(AttackSpeed);
        hitEnemy.Remove(monster);
    }
}
