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
    
    [SerializeField] private Tilemap _tilemapPlayArea;
    [SerializeField] private Tilemap _tilemapPlayingElements;
    [SerializeField] private List<TileBase> _playingElements;
    
    private Unity.Mathematics.Random _random;

    void Start()
    {
        _random = new Unity.Mathematics.Random(Convert.ToUInt32(DateTime.Now.Millisecond));
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
                    var cell = new Cell(pos);
                    cells.Add(pos, cell);
                }
            }
        }
        
        GlobalEventManager.LoadLevel.Invoke(cells,_random,_tilemapPlayingElements,_playingElements[0]);
        GlobalEventManager.SendPlaingArea.Invoke(_tilemapPlayArea);
    }
}
