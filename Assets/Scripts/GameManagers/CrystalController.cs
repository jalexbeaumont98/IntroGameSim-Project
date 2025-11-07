using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    // Static event that other scripts can subscribe to
    

    private int currentHP = 0;
    public int maxHP = 10;

    public static event System.Action<int, int> OnCrystalDamage;

    void Start()
    {
        currentHP = maxHP;
        OnCrystalDamage?.Invoke(currentHP, maxHP);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        OnCrystalDamage?.Invoke(currentHP, maxHP);
    }

    


}
