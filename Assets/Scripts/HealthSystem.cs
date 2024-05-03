using System;
using UnityEngine;
using UnityEngine.Serialization;


public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health = 100;
    private float healthMax;
    
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    public void Awake()
    {
        healthMax = health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }

        OnDamaged?.Invoke(this,EventArgs.Empty);
        if (health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDead?.Invoke(this,EventArgs.Empty);
    }

    public float GetHealthNormalized() => health / healthMax;
}
