using System;
using System.Collections;
using System.Collections.Generic;
using Clicker;
using UnityEngine;

public class Earnings : MonoBehaviour
{
    public static Action<Earnings> SendEarnings;

    [SerializeField] private float _accrualInterval;
    
    public int Money { get; private set; }
    
    private Dictionary<Vector3Int, Cell> _profitableCells = new Dictionary<Vector3Int, Cell>();
    private float _lastAccrual;
    
    void Start()
    {
        Money = 0;
        _lastAccrual = Time.time;
        SendEarnings.Invoke(gameObject.GetComponent<Earnings>());
    }

    void Update()
    {
        if (Time.time > _lastAccrual + _accrualInterval)
        {
            int sum = 0;
            foreach (var cell in _profitableCells)
            {
                switch (cell.Value.CellStatus)
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
                    case CellStatus.Empty:
                        throw new Exception($"ProfitableCell in key {cell.Key} is enpty");
                        break;
                }
            }

            Money += sum;
            _lastAccrual = Time.time;
        }
    }

    private void OnEnable()
    {
        ObjectGenerator.AddProfitableCells += AddCell;
    }

    private void OnDisable()
    {
        ObjectGenerator.AddProfitableCells -= AddCell;
    }

    private void AddCell(Vector3Int position, Cell cell)
    {
        if (!_profitableCells.ContainsKey(position))
        {
            _profitableCells.Add(position,cell);
        }
        else
        {
            _profitableCells[position] = cell;
        }
    }

    private void RemoveCell(Vector3Int position)
    {
        if (_profitableCells.ContainsKey(position))
        {
            _profitableCells.Remove(position);
        }
        else
        {
            throw new Exception($"Earning dictionary don't have cell with key {position}");
        }
    }
}
