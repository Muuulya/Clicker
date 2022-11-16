using System;
using System.Collections.Generic;
using System.Linq;
using Clicker;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = Unity.Mathematics.Random;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private TilemapsData _tilemapsData;
    [SerializeField] private int _maxFilledCells;
    [SerializeField] private float _spawnDelay;

    private Dictionary<Vector3Int, Cell> _cells;
    private Random _random;
    private float _lastSpawnTime = 0;

    private void Start()
    {
        _random = new Random(Convert.ToUInt32(DateTime.Now.Millisecond));
        SpawnNewTile();
    }
    
    private void Update()
    {
        if (Time.time > _lastSpawnTime + _spawnDelay)
        {
            SpawnNewTile();
            _lastSpawnTime = Time.time;
        }
    }

    public void SpawnNewTile()
    {
        var filledCells = _tilemapsData.GetFilledCells();
        
        if (filledCells.Count < _maxFilledCells)
        {
            var emptyCellsCount = _tilemapsData.GetEmptyCells();
            
            if (emptyCellsCount.Count > 0)
            {
                var pos = emptyCellsCount.ElementAt(_random.NextInt(0, emptyCellsCount.Count)).Key;
                _tilemapsData.SetTile(pos, CellStatus.Lv1);
            }
        }
    }
}
