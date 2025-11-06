using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected LayerMask groundLayer;

    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    [SerializeField] protected GameObject hitEffect;
    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] protected Rigidbody2D rb;


    public virtual void Initialize(ProjectileData data)
    {
        speed = data.speed;
        damage = data.damage;

        if (data.hitEffect)
            hitEffect = data.hitEffect;

        sr = GetComponent<SpriteRenderer>();
        if (data.sprite)
            sr.sprite = data.sprite;

        rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, 12);

    }
    
    public virtual void Launch()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        print("collided with trigger: " + collision.gameObject.tag);
        
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            DestroyProjectile();
        }

    }
    
    public virtual void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
