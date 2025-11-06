using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootController : MonoBehaviour
{
    public Transform firePoint;

    public SpriteRenderer sr;
    public Transform pointLeft;
    public Transform pointRight;

    public ProjectileData projData;

    void Start()
    {
        
    }

    public void SetFlipped(bool flipped)
    {
        if (sr == null) return;

        if (!flipped)
        {
            sr.flipX = false;
            transform.position = pointLeft.position;
        }

        else
        {
            sr.flipX = true;
            transform.position = pointRight.position;
        }
    }

    public void FireProjectile()
    {
        if (projData == null) return;

        // Get world position of the mouse
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // Compute direction from fire point to mouse
        Vector2 dir = (mouseWorldPos - firePoint.position).normalized;

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
}
