using System;
using System.Collections;
using System.Collections.Generic;
using Clicker;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Random = Unity.Mathematics.Random;

public class ClickTrecking : MonoBehaviour, IPointerClickHandler, 
    IDragHandler, IEndDragHandler, IBeginDragHandler
{

    private Camera _mainCamera;
    private bool _isDrag = false;

    private Tilemap _playArea;
    private Tilemap _PlayingElements;
    private Dictionary<Vector3Int, Cell> _playingObjects = new Dictionary<Vector3Int, Cell>();
    private GameObject _dragableObject;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        GlobalEventManager.AddFilledCell.AddListener(AddCell);
        GlobalEventManager.RemoveFilledCell.AddListener(RemoveCell);
        GlobalEventManager.SendPlaingArea.AddListener(AddPlayArea);
        GlobalEventManager.SendPlaingElements.AddListener(AddPlaingElements);
    }

    private void AddCell(Vector3Int position, Cell cell)
    {
        if (!_playingObjects.ContainsKey(position))
        {
            _playingObjects.Add(position,cell);
        }
        else
        {
            _playingObjects[position] = cell;
        }
    }

    private void RemoveCell(Vector3Int position)
    {
        if (_playingObjects.ContainsKey(position))
        {
            _playingObjects.Remove(position);
        }
        else
        {
            throw new Exception("ClickTrecking dictionary don't have cell with key {position}");
        }
    }

    private void AddPlayArea(Tilemap tilemap)
    {
        _playArea = tilemap;
    }

    private void AddPlaingElements(Tilemap tilemap)
    {
        _PlayingElements = tilemap;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isDrag)
        {
            var clickWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var clickCellPosition = _playArea.WorldToCell(clickWorldPosition);
            
            if (_playingObjects.ContainsKey(clickCellPosition))
            {
                GlobalEventManager.AccrueMoney.Invoke(_playingObjects[clickCellPosition]);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // _dragableObject.transform.position = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var pos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        _dragableObject.transform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(_dragableObject);
        _isDrag = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var clickWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var clickCellPosition = _playArea.WorldToCell(clickWorldPosition);
        
        if (_playingObjects[clickCellPosition].CellStatus != CellStatus.Empty)
        {
            _isDrag = true;
            var sprite = _PlayingElements.GetSprite(clickCellPosition);
            var go = new GameObject();
            go.AddComponent<SpriteRenderer>();
            go.transform.position = Vector3.zero;
            _dragableObject = go;
            // _dragableObject = Instantiate(sprite, Input.mousePosition, Quaternion.identity);
            // _dragableObject = Instantiate(go,Input.mousePosition,Quaternion.identity);
            _dragableObject.GetComponent<SpriteRenderer>().sprite = sprite;
            _dragableObject.GetComponent<SpriteRenderer>().sortingOrder = 5;

        }
    }
}
