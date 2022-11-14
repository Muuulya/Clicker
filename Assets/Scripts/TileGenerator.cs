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

    // [SerializeField] private TileBase _tileLevel1;
    // [SerializeField] private TileBase _tileLevel2;
    // [SerializeField] private TileBase _tileLevel3;
    // [SerializeField] private TileBase _tileLevel4;
    // [SerializeField] private TileBase _tileLevel5;
    
    // private Dictionary<CellStatus, TileBase> _tiles;

    private Dictionary<Vector3Int, Cell> _cells;
    private Random _random;
    private float _lastSpawnTime = 0;


    private void Start()
    {
        _random = new Random(Convert.ToUInt32(DateTime.Now.Millisecond));
        // _tiles = new Dictionary<CellStatus, TileBase>();
        // _tiles.Add(CellStatus.Lv1,_tileLevel1);
        // _tiles.Add(CellStatus.Lv2,_tileLevel2);
        // _tiles.Add(CellStatus.Lv3,_tileLevel3);
        // _tiles.Add(CellStatus.Lv4,_tileLevel4);
        // _tiles.Add(CellStatus.Lv5,_tileLevel5);
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
