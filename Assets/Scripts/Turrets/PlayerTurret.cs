using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurret : MonoBehaviour
{
    [Header("Turret Settings")]
    [SerializeField] Transform firePoint;
    [SerializeField] ProjectileData projData;

    [Header("Firing Settings")]
    [SerializeField] protected float fireRate = 2f;  // shots per second — higher = faster
    protected float nextFireTime = 0f;
    [SerializeField] public float range = 5f;
    private Transform currentTarget;



    private CircleCollider2D rangeCollider;

    private void Awake()
    {
        rangeCollider = GetComponent<CircleCollider2D>();
        rangeCollider.isTrigger = true;
        rangeCollider.radius = range;
    }

    private void Update()
    {
        // If the current target was destroyed, clear and look for another
        if (currentTarget == null)
        {
            FindNewTarget();
        }

        else
        {
            if (Time.time >= nextFireTime)
            {
                ShootEnemy();

                // Set next allowed time to fire
                nextFireTime = Time.time + (1f / fireRate);
            }
        }
    }

    private void ShootEnemy()
    {
        if (projData == null) return;

        // Get world position of the mouse
        Vector3 targetPos = currentTarget.position;
        targetPos.z = 0f;

        // Compute direction from fire point to mouse
        Vector2 dir = (targetPos - firePoint.position).normalized;

        // Calculate rotation angle (for a 2D top-down or side view)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Instantiate and rotate projectile
        GameObject projInstance = Instantiate(projData.prefab, firePoint.position, Quaternion.Euler(0, 0, angle));

        // Initialize it
        Projectile proj = projInstance.GetComponent<Projectile>();
        proj.Initialize(projData);

        // Launch it in the direction it’s facing
        proj.Launch();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        // If we don't have a target, this becomes our target
        if (currentTarget == null)
        {
            currentTarget = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == currentTarget)
        {
            currentTarget = null;
            FindNewTarget();
        }
    }

    private void FindNewTarget()
    {
        // Check all colliders in range and pick the first enemy
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                currentTarget = hit.transform;
                return;
            }
        }

        currentTarget = null; // none found
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
