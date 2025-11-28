using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurret : MonoBehaviour
{
    [Header("Turret Settings")]
    [SerializeField] Transform firePoint;
    [SerializeField] ProjectileData projData;
    [SerializeField] private LayerMask groundMask;

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
        if (IsTargetBlocked(currentTarget))
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
        AudioController.Instance.PlaySound_PlayerShoot();
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
                if (!IsTargetBlocked(hit.transform))
                {
                    currentTarget = hit.transform;
                    return;
                }

            }
        }

        currentTarget = null; // none found
    }

    public bool IsTargetBlocked(Transform target)
    {
        if (target == null) return true; // treat null as blocked

        Vector2 origin = firePoint.position;            // wherever the shot originates
        Vector2 direction = (target.position - firePoint.position).normalized;
        float distance = Vector2.Distance(firePoint.position, target.position);

        // Raycast toward target, but only for ground layer
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, groundMask);

        #if UNITY_EDITOR
        // Debug draw the ray
        Color rayColor = (hit.collider == null) ? Color.green : Color.red;
        Debug.DrawLine(origin, target.position, rayColor);
        #endif

        // If hit something in ground layer → target is blocked
        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
