using System;
using System.Collections.Generic;
using System.Linq;
using Clicker;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = Unity.Mathematics.Random;

public class ObjectGenerator : MonoBehaviour
{
    public static Action<Vector3Int, Cell> AddProfitableCells;
    public static Action<Vector3Int> RemoveProfitableCells;
    
    private Tilemap _tilemapPlayingElements;
    private TileBase _newTile;

    [SerializeField] private int _maxFilledCells;
    [SerializeField] private float _spawnDelay;

    private Dictionary<Vector3Int, Cell> _cells;
    private Random _random;
    private float _lastSpawnTime = 0;

    private void OnEnable()
    {
        LevelLoader.LoadLevel += LoadLevl;
    }

    private void OnDisable()
    {
        LevelLoader.LoadLevel -= LoadLevl;
    }
    
    private void LoadLevl(Dictionary<Vector3Int, Cell> cells, Random random,
        Tilemap tilemap, TileBase tile)
    {
        _cells = cells;
        _random = random;
        _tilemapPlayingElements = tilemap;
        _newTile = tile;
    }
    
    private void Update()
    {
        if (Time.time > _lastSpawnTime + _spawnDelay)
        {
            SpawnNewTile();
            _lastSpawnTime = Time.time;
        }
    }

    private void SpawnNewTile()
    {
        var filledCellsCount = (from cell in _cells
            where cell.Value.CellStatus != CellStatus.Empty
            select cell).ToList().Count;
            
        if (filledCellsCount < _maxFilledCells)
        {
            var emptyCellsCount = (from cell in _cells
                    where cell.Value.CellStatus == CellStatus.Empty
                    select cell)
                .ToDictionary(cell => cell.Key, cell => cell.Value);
                
            if (emptyCellsCount.Count > 0)
            {
                var pos = emptyCellsCount.ElementAt(_random.NextInt(0, emptyCellsCount.Count)).Key;
                _tilemapPlayingElements.SetTile(pos,_newTile);
                _cells[pos].CellStatus = CellStatus.Lv1;
                AddProfitableCells.Invoke(pos,_cells[pos]);
            }
        }
    }
}
