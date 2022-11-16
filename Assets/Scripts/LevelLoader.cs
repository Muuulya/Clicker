using System;
using System.Collections;
using System.Collections.Generic;
using Clicker;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelLoader : MonoBehaviour
{
    // public static Action<Dictionary<Vector3Int, Cell>, Unity.Mathematics.Random, 
    //     Tilemap, TileBase> LoadLevel;
    
    [SerializeField] private TilemapsData _tilemapsData;
    [SerializeField] private Tilemap _tilemapPlayArea;
    [SerializeField] private Tilemap _tilemapPlayingElements;

    void Start()
    {
        _tilemapPlayArea.CompressBounds();
        var minPosition = _tilemapPlayArea.cellBounds.min;
        var maxPosition = _tilemapPlayArea.cellBounds.max;
        
        var cells = new Dictionary<Vector3Int, Cell>();
        
        for (int i = minPosition.x; i < maxPosition.x; i++)
        {
            for (int j = minPosition.y; j < maxPosition.y; j++)
            {
                var pos = new Vector3Int(i, j, 0);
                if (_tilemapPlayArea.GetTile(pos) != null)
                {
                    var worldPos = _tilemapPlayArea.CellToWorld(pos);
                    var cell = new Cell(pos, worldPos);
                    cells.Add(pos, cell);
                }
            }
        }
        _tilemapsData.InitializeTilemapsData(_tilemapPlayArea,_tilemapPlayingElements,cells);
    }
    
}
