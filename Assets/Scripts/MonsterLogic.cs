﻿using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLogic : MonoBehaviour
{
    public float MaxHealth = 50;

    public float AttackDamage = 10f;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.parent.tag == "Chest")
        {

        }
    }
}
