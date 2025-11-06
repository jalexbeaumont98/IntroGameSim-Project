using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{

    protected override void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy)
            {
                enemy.TakeDamage(damage);
            }

            DestroyProjectile();
        }
        
        base.OnTriggerEnter2D(collision);
    }

    
}