using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player)
            {
                player.TakeDamage(damage);
            }

            DestroyProjectile();
        }
        
        base.OnTriggerEnter2D(collision);
    }
}
