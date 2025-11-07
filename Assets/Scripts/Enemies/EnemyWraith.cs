using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWraith : Enemy
{
    [Header("References")]
    [SerializeField] ProjectileData projData;
    [SerializeField] Transform firePoint;
    [SerializeField] private LayerMask groundLayer;


    [Header("Movement")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float arrivalThreshold = 0.1f;

    
    
    [SerializeField] private bool stopMoving = false;                     // set true when final waypoint reached

    [Header("Player Detection")]

    [SerializeField] private float detectRadius = 2f;
    private bool pausedByPlayer = false;

    protected override void Start()
    {
        base.Start();

    }

    

    private void Update()
    {
        if (stopMoving)
        {
            Attack(crystal);
            return;
        }


        // Pause when player is close
        pausedByPlayer = (player != null) && (Vector2.Distance(transform.position, player.position) <= detectRadius);
        if (pausedByPlayer)
        {
            float distToPlayer = Vector2.Distance(transform.position, player.position);

            // Raycast from firePoint toward player
            Vector2 dirToPlayer = (player.position - firePoint.position).normalized;
            float rayDist = distToPlayer; // only check as far as player

            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, dirToPlayer, rayDist, groundLayer);

            // If the raycast hits nothing on the ground layer → line of sight is clear
            if (hit.collider == null)
            {
                Attack(player);
                return;
            }


        }

        if (currentWaypoint == null) return;

        // Move toward current waypoint
        Vector3 wp = currentWaypoint.position;
        transform.position = Vector3.MoveTowards(transform.position, wp, speed * Time.deltaTime);

        // Close enough → ask for next
        if (Vector2.Distance(transform.position, wp) <= arrivalThreshold)
        {
            print("arrived at current waypoint");
            Transform next = GameManager.Instance.GetNextWaypoint(pathIndex);
            pathIndex++;

            // If same waypoint returned → final destination reached
            if (next == currentWaypoint)
            {
                stopMoving = true;
            }
            else
            {
                currentWaypoint = next;
            }
        }

        FaceDirection();

    }

    private void FaceDirection()
    {
        if (currentWaypoint == null) return;

        // Compare X positions to determine facing direction
        if (currentWaypoint.position.x > transform.position.x)
        {
            // Waypoint is to the right → face right
            sr.flipX = false;
        }
        else if (currentWaypoint.position.x < transform.position.x)
        {
            // Waypoint is to the left → face left
            sr.flipX = true;
        }
    }

    private void Attack(Transform target)
    {
        // Compare X positions to determine facing direction
        if (target.position.x > transform.position.x)
        {
            // Waypoint is to the right → face right
            sr.flipX = false;
        }
        else if (target.position.x < transform.position.x)
        {
            // Waypoint is to the left → face left
            sr.flipX = true;
        }

        if (Time.time >= nextFireTime)
        {
            FireProjectile(target);

            // Set next allowed time to fire
            nextFireTime = Time.time + (1f / fireRate);
        }


    }

    void FireProjectile(Transform target)
    {
        if (projData == null) return;

        // Get world position of the mouse
        Vector3 targetPos = target.position;
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

    private void OnDrawGizmosSelected()
    {
        // Player detect radius
        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        // Waypoint link
        if (currentWaypoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, currentWaypoint.position);
            Gizmos.DrawWireSphere(currentWaypoint.position, 0.15f);
        }
    }
}
