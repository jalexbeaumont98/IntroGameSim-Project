using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMoneyUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    void OnEnable()
    {
        GameManager.OnPlayerMoneyChange += SetMoneyText;
    }

    void OnDisable()
    {
        GameManager.OnPlayerMoneyChange -= SetMoneyText;
    }
    
    void SetMoneyText(int money)
    {
        text.text = "Money: " + money;
    }
}
