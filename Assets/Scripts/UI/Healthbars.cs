using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbars : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Slider crystalSlider;
    [SerializeField] private Slider playerSlider;

    void OnEnable()
    {
        CrystalController.OnCrystalDamage += UpdateCrystalHP;
        PlayerController.OnPlayerHPChange += UpdatePlayerHP;
    }

    void OnDisable()
    {
        CrystalController.OnCrystalDamage -= UpdateCrystalHP;
        PlayerController.OnPlayerHPChange -= UpdatePlayerHP;
    }

    private void UpdateCrystalHP(int currentHP, int maxHP)
    {
        crystalSlider.value = currentHP;
        crystalSlider.maxValue = maxHP;
    } 
    
    private void UpdatePlayerHP(int currentHP, int maxHP)
    {
        playerSlider.value = currentHP;
        playerSlider.maxValue = maxHP;
    }
}
