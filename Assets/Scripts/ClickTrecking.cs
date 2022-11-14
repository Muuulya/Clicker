using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Clicker;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Random = Unity.Mathematics.Random;

public class ClickTrecking : MonoBehaviour, IPointerClickHandler, 
    IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private TileGenerator _tileGenerator;
    [SerializeField] private TilemapsData _tilemapsData;

    private Camera _mainCamera;
    private bool _isDrag = false;

    private Cell _currentDragableCell;
    private GameObject _dragableObject;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isDrag)
        {
            var filledCells = _tilemapsData.GetFilledCells();
            
            var clickWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var clickCellPosition = _tilemapsData.GetCellPosition(clickWorldPosition);
            
            if (filledCells.ContainsKey(clickCellPosition))
            {
                GlobalEventManager.AccrueMoney.Invoke(filledCells[clickCellPosition]);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        var pos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        _dragableObject.transform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var filledCells = _tilemapsData.GetFilledCells();

        var clickWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var clickCellPosition = _tilemapsData.GetCellPosition(clickWorldPosition);

        if (filledCells.ContainsKey(clickCellPosition) &&
            filledCells[clickCellPosition].CellStatus == _currentDragableCell.CellStatus
            && filledCells[clickCellPosition].CellStatus != CellStatus.Lv5)
        {
            switch (_currentDragableCell.CellStatus)
            {
                case CellStatus.Lv1:
                    _tilemapsData.SetTile(clickCellPosition,CellStatus.Lv2);
                    break;
                case CellStatus.Lv2:
                    _tilemapsData.SetTile(clickCellPosition,CellStatus.Lv3);
                    break;
                case CellStatus.Lv3:
                    _tilemapsData.SetTile(clickCellPosition,CellStatus.Lv4);
                    break;
                case CellStatus.Lv4:
                    _tilemapsData.SetTile(clickCellPosition,CellStatus.Lv5);
                    break;
            }
            _tilemapsData.SetTile(_currentDragableCell.Position,CellStatus.Empty);
        }
        else
        {
            _tilemapsData.SetTile(_currentDragableCell);
        }

        Destroy(_dragableObject);
        _currentDragableCell = null;
        _isDrag = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var filledCells = _tilemapsData.GetFilledCells();

        var clickWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var clickCellPosition = _tilemapsData.GetCellPosition(clickWorldPosition);
        
        if (filledCells[clickCellPosition].CellStatus != CellStatus.Empty)
        {
            _isDrag = true;
            var sprite = _tilemapsData.GetSprite(clickCellPosition);
            var go = new GameObject();
            go.AddComponent<SpriteRenderer>();
            go.transform.position = Vector3.zero;
            _dragableObject = go;
            // _dragableObject = Instantiate(sprite, Input.mousePosition, Quaternion.identity);
            // _dragableObject = Instantiate(go,Input.mousePosition,Quaternion.identity);
            _dragableObject.GetComponent<SpriteRenderer>().sprite = sprite;
            _dragableObject.GetComponent<SpriteRenderer>().sortingOrder = 5;

            _currentDragableCell = new Cell(filledCells[clickCellPosition]);
            _tilemapsData.SetTile(clickCellPosition,CellStatus.Drag);
        }
    }
}
