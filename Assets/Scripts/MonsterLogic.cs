﻿using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLogic : MonoBehaviour
{
    public float MaxHealth = 50;

    public float AttackDamage = 10f;

    public float AttackSpeed = 1 / 1f;

    private HealthBar healthbar;

    private float health;
    public float Health
    {
        get { return health; }
        set
        {
            health = value;
            if (healthbar != null)
            {
                healthbar.SetSize(value / MaxHealth);
            }
        }
    }

    private bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        healthbar = GetComponentInChildren<HealthBar>();
        Health = MaxHealth;
    }

    public void Damage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Transform collideWith = collision.transform;
        if (collideWith.transform.parent != null)
            collideWith = collideWith.transform.parent;

        if (collideWith.tag == "Chest" )
        {
            if (canAttack)
            {
                GameHandler.instance.DamageChest(AttackDamage);
                StartCoroutine(WaitForAttack());
            }
        }
        else if (collideWith.tag == "Turret")
        {
            
            if (canAttack)
            {
                collideWith.GetComponent<Damageable>().Damage(AttackDamage);
                StartCoroutine(WaitForAttack());
            }
        }
    }

    IEnumerator WaitForAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(AttackSpeed);
        canAttack = true;
    }
}
