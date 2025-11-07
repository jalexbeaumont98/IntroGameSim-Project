using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int maxHP;
    [SerializeField] protected Transform crystal;
    [SerializeField] protected Transform player;
    [SerializeField] protected SpriteRenderer sr;

    [Header("Firing Settings")]
    [SerializeField] protected float fireRate = 2f;  // shots per second — higher = faster
    protected float nextFireTime = 0f;

    [Header("Waypoints")]
    [SerializeField] protected Transform currentWaypoint;  // optional start; if null, GameState will give first
    [SerializeField] protected int pathIndex = -2;

    [SerializeField] private int path = 0;
    [SerializeField] int deathValue = 10;
    protected int currentHP;

    protected virtual void Start()
    {
        currentHP = maxHP;
    }

    public void InitializeEnemy()
    {
        // Ensure we have a starting waypoint from GameState
        if (GameManager.Instance == null)
        {
            Debug.LogError("GhostPathFollower: No GameState in scene.");
            enabled = false;
            return;
        }

        print("asking for waypoint");

        if (currentWaypoint == null)
        {
            currentWaypoint = GameManager.Instance.GetNextWaypoint(-2, path);
            pathIndex++;
        }

        crystal = GameObject.FindGameObjectWithTag("Crystal").transform;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        sr = GetComponent<SpriteRenderer>();
    }

    public void SetPath(int input)
    {
        path = input;
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
        GameManager.Instance.SetMoney(deathValue);
        GameManager.Instance.SubtractEnemy();
        Destroy(gameObject);
    }
}
