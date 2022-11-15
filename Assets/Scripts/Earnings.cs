using System;
using System.Collections;
using System.Collections.Generic;
using Clicker;
using UnityEngine;

public class Earnings : MonoBehaviour
{
    [SerializeField] private TilemapsData _tilemapsData;
    [SerializeField] private float _accrualInterval;
    [SerializeField] private GameObject _coin;
    [SerializeField] private Transform _coinText;
    [SerializeField] private CoinSpawner _coinSpawner;

        public int Money { get; private set; }
    
    private float _lastAccrual;
    
    void Start()
    {
        Money = 0;
        _lastAccrual = Time.time;
        GlobalEventManager.SendEarnings.Invoke(gameObject.GetComponent<Earnings>());
    }

    private void AccrueMoney(Cell cell)
    {
        int sum = 0;
        switch (cell.CellStatus)
        {
            case CellStatus.Lv1:
                sum += 1;
                break;
            case CellStatus.Lv2:
                sum += 2;
                break;
            case CellStatus.Lv3:
                sum += 4;
                break;
            case CellStatus.Lv4:
                sum += 8;
                break;
            case CellStatus.Lv5:
                sum += 16;
                break;
        }

        // var coin = Instantiate(_coin, cell.WorldPosition, Quaternion.identity);
        // var aa = _coinText.position;
        // aa.z = 6;
        // var a = Camera.main.ScreenToWorldPoint(aa);
        // coin.GetComponent<Coin>().Initialize(a);
        _coinSpawner.SpawnCoin(cell.WorldPosition);
        Money += sum;
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

    private void OnEnable()
    {
        GlobalEventManager.AccrueMoney.AddListener(AccrueMoney);
    }
}
