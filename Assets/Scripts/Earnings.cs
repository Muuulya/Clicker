using System;
using System.Collections;
using System.Collections.Generic;
using Clicker;
using UnityEngine;

public class Earnings : MonoBehaviour
{
    [SerializeField] private TilemapsData _tilemapsData;
    public static Action<Earnings> SendEarnings;

    [SerializeField] private float _accrualInterval;
    
    public int Money { get; private set; }
    
    // private Dictionary<Vector3Int, Cell> _profitableCells = new Dictionary<Vector3Int, Cell>();
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
            case CellStatus.Empty:
                throw new Exception($"ProfitableCell in key {cell} is enpty");
                break;
        }

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
        // GlobalEventManager.AddFilledCell.AddListener(AddCell);
        // GlobalEventManager.RemoveFilledCell.AddListener(RemoveCell);
        GlobalEventManager.AccrueMoney.AddListener(AccrueMoney);
    }
    
    // private void AddCell(Vector3Int position, Cell cell)
    // {
    //     if (!_profitableCells.ContainsKey(position))
    //     {
    //         _profitableCells.Add(position,cell);
    //     }
    //     else
    //     {
    //         _profitableCells[position] = cell;
    //     }
    // }
    //
    // private void RemoveCell(Vector3Int position)
    // {
    //     if (_profitableCells.ContainsKey(position))
    //     {
    //         _profitableCells.Remove(position);
    //     }
    //     else
    //     {
    //         throw new Exception($"Earning dictionary don't have cell with key {position}");
    //     }
    // }
}
