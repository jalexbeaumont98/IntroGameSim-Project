using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileData", menuName = "Game Data/Projectile")]
public class ProjectileData : ScriptableObject
{
    [Header("Stats")]
    public float speed = 10f;
    public int damage = 10;

    [Header("Visuals")]
    public Sprite sprite;
    public GameObject prefab;
    public GameObject hitEffect; // particle or explosion prefab
}
