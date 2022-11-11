using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderPanel : MonoBehaviour
{
    [SerializeField] private Text _moneyText;
    
    private Earnings _earnings;
    
    void Start()
    {
        _moneyText.text = "0";
    }

    void Update()
    {
        _moneyText.text = _earnings.Money.ToString();
    }

    private void OnEnable()
    {
        Earnings.SendEarnings += AddEarnings;
    }

    private void OnDisable()
    {
        Earnings.SendEarnings -= AddEarnings;
    }

    private void AddEarnings(Earnings earnings)
    {
        _earnings = earnings;
    }
}
