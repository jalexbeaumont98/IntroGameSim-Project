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


    bool projectileSet;

    protected virtual void Start()
    {

    }
    
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

        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            DestroyProjectile();
        }
    }
    
    public virtual void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
