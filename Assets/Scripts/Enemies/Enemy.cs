using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int maxHP;
    [SerializeField] protected Transform crystal;

    [Header("Firing Settings")]
    [SerializeField] protected float fireRate = 2f;  // shots per second — higher = faster
    protected float nextFireTime = 0f;


    protected int currentHP;

    protected virtual void Start()
    {
        currentHP = maxHP;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            DieEnemy();
        }
    }
    
    protected virtual void DieEnemy()
    {
        Destroy(gameObject);
    }
}
