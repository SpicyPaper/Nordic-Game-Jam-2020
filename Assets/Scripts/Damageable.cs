using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float MaxHealth = 30f;

    public HealthBar healthbar;

    private float currentHealth;
    public float CurrentHealth { get => currentHealth; set { currentHealth = value; healthbar.SetSize(CurrentHealth / MaxHealth); } }

    void Start()
    {
        CurrentHealth = MaxHealth;    
    }
    

    public void Damage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
