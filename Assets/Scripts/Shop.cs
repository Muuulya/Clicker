using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{    
    public int CurrentCellPrice { get; private set; }

    [SerializeField] private TileGenerator _tileGenerator;
    [SerializeField] private Earnings _earnings;
    [SerializeField] private Text _cellPriceText;
    [SerializeField] private int _startCellPrice = 200;
    
    public void BuyNewCell()
    {
        if (_earnings.Money > CurrentCellPrice)
        {
            _earnings.SpendMoney(CurrentCellPrice);
            _tileGenerator.AddCell();
            CurrentCellPrice *= 3;
        }
    }

    public void InitializeShop()
    {
        CurrentCellPrice = _startCellPrice;
    }

    public void InitializeShop(int currentCellPrice)
    {
        CurrentCellPrice = currentCellPrice;
    }

    void Update()
    {
        _cellPriceText.text = CurrentCellPrice.ToString();
    }
}
