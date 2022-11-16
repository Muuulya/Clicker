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
    [SerializeField] private TilemapsData _tilemapsData;
    [SerializeField] private Earnings _earnings;
    [SerializeField] private float _DragOffset = 1;

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
            
            var clickWorldPosition = _mainCamera.ScreenToWorldPoint(GetMousPosition());
            var clickCellPosition = _tilemapsData.GetCellPosition(clickWorldPosition);
            
            if (filledCells.ContainsKey(clickCellPosition))
            {
                _earnings.AccrueMoney(filledCells[clickCellPosition]);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDrag)
        {
            var clickWorldPosition = _mainCamera.ScreenToWorldPoint(GetMousPosition());
            clickWorldPosition.y += _DragOffset;
            _dragableObject.transform.position = clickWorldPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isDrag)
        {
            var filledCells = _tilemapsData.GetFilledCells();

            var clickWorldPosition = _mainCamera.ScreenToWorldPoint(GetMousPosition());
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
        }
        _isDrag = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var filledCells = _tilemapsData.GetFilledCells();
        
        var clickWorldPosition = _mainCamera.ScreenToWorldPoint(GetMousPosition());
        var clickCellPosition = _tilemapsData.GetCellPosition(clickWorldPosition);
        
        if (filledCells.ContainsKey(clickCellPosition))
        {
            _isDrag = true;
            _dragableObject = new GameObject();
            var spriteRenderer = _dragableObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = _tilemapsData.GetSprite(clickCellPosition);
            spriteRenderer.sortingOrder = 5;
            clickWorldPosition.y += _DragOffset;
            _dragableObject.transform.position = clickWorldPosition;

            _currentDragableCell = new Cell(filledCells[clickCellPosition]);
            _tilemapsData.SetTile(clickCellPosition,CellStatus.Drag);
        }
    }

    private Vector3 GetMousPosition()
    {
        var mousePosition = Input.mousePosition;
        var z = (gameObject.transform.position - _mainCamera.transform.position).z;
        mousePosition.z = z;
        return mousePosition;
    }
}
