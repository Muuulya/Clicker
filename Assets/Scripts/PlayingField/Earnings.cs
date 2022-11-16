using System;
using System.Collections;
using System.Collections.Generic;
using Clicker;
using UnityEngine;

public class Earnings : MonoBehaviour
{
    [SerializeField] private TilemapsData _tilemapsData;
    [SerializeField] private float _accrualInterval;
    [SerializeField] private CoinSpawner _coinSpawner;

    public int Money { get; private set; }
    
    private float _lastAccrual;

    public void Initialize(int money)
    {
        Money = money;
        _lastAccrual = Time.time;
    }

    public void AccrueCoinsForPastPeriod(int period)
    {
        var numberOfAccrual = (int)(period / _accrualInterval);
        var cells = _tilemapsData.GetFilledCells();
        foreach (var cell in cells)
        {
            switch (cell.Value.CellStatus)
            {
                case CellStatus.Lv1:
                    Money += 1 * numberOfAccrual;
                    break;
                case CellStatus.Lv2:
                    Money += 2 * numberOfAccrual;
                    break;
                case CellStatus.Lv3:
                    Money += 4 * numberOfAccrual;
                    break;
                case CellStatus.Lv4:
                    Money += 8 * numberOfAccrual;
                    break;
                case CellStatus.Lv5:
                    Money += 16 * numberOfAccrual;
                    break;
            }

        }
    }

    public void AccrueMoney(Cell cell)
    {
        switch (cell.CellStatus)
        {
            case CellStatus.Lv1:
                Money += 1;
                _coinSpawner.SpawnCoin(cell.WorldPosition, 1);
                break;
            case CellStatus.Lv2:
                Money += 2;
                _coinSpawner.SpawnCoin(cell.WorldPosition, 2);
                break;
            case CellStatus.Lv3:
                Money += 4;
                _coinSpawner.SpawnCoin(cell.WorldPosition, 4);
                break;
            case CellStatus.Lv4:
                Money += 8;
                _coinSpawner.SpawnCoin(cell.WorldPosition, 8);
                break;
            case CellStatus.Lv5:
                Money += 16;
                _coinSpawner.SpawnCoin(cell.WorldPosition, 16);
                break;
        }
    }
    void Update()
    {
        if (Time.time > _lastAccrual + _accrualInterval)
        {
            int sum = 0;
            foreach (var cell in _tilemapsData.GetFilledCells())
            {
                AccrueMoney(cell.Value);
            }
            
            _lastAccrual = Time.time;
        }
    }
}
