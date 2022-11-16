using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Clicker;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapsData : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] private TileBase _tileEmpty;
    [SerializeField] private TileBase _tileLevel1;
    [SerializeField] private TileBase _tileLevel2;
    [SerializeField] private TileBase _tileLevel3;
    [SerializeField] private TileBase _tileLevel4;
    [SerializeField] private TileBase _tileLevel5;
    
    [Header("Tilemaps")]
    [SerializeField] private Tilemap _tilemapPlayArea; 
    [SerializeField] private Tilemap _tilemapPlayingElements; 
    
    private Dictionary<Vector3Int, Cell> _cells;
    private Dictionary<CellStatus, TileBase> _tiles;
    
    public void Initialize(Dictionary<Vector3Int, Cell> cells)
    {
        _cells = cells;
        
        _tiles = new Dictionary<CellStatus, TileBase>();
        _tiles.Add(CellStatus.Empty,_tileEmpty);
        _tiles.Add(CellStatus.Lv1,_tileLevel1);
        _tiles.Add(CellStatus.Lv2,_tileLevel2);
        _tiles.Add(CellStatus.Lv3,_tileLevel3);
        _tiles.Add(CellStatus.Lv4,_tileLevel4);
        _tiles.Add(CellStatus.Lv5,_tileLevel5);
        _tiles.Add(CellStatus.Drag,_tileEmpty);

        foreach (var cell in _cells)
        {
            SetTile(cell.Value);
        }
    }

    public Dictionary<Vector3Int, Cell> GetAllCells()
    {
        var cells = _cells
            .ToDictionary(c => c.Key, c => c.Value);
        return cells;
    }

    public Dictionary<Vector3Int, Cell> GetEmptyCells()
    {
        return (from cell in _cells
                where cell.Value.CellStatus == CellStatus.Empty
                select cell)
            .ToDictionary(c => c.Key, c => c.Value);
    }
    
    public Dictionary<Vector3Int,Cell> GetFilledCells()
    {
        return (from cell in _cells
            where cell.Value.CellStatus != CellStatus.Empty && cell.Value.CellStatus != CellStatus.Drag
            select cell)
            .ToDictionary(c => c.Key, c=> c.Value);
    }

    public void SetTile(Vector3Int position, CellStatus status)
    {
        _tilemapPlayingElements.SetTile(position,_tiles[status]);
        _cells[position].CellStatus = status;
    }
    
    public void SetTile(Cell cell)
    {
        _tilemapPlayingElements.SetTile(cell.Position,_tiles[cell.CellStatus]);
        _cells[cell.Position].CellStatus = cell.CellStatus;
    }

    public Vector3Int GetCellPosition(Vector3 worldPosition)
    {
        return _tilemapPlayArea.WorldToCell(worldPosition);
    }

    public Sprite GetSprite(Vector3Int position)
    {
        return _tilemapPlayingElements.GetSprite(position);
    }
}
