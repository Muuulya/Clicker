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
    private Dictionary<Vector3Int, Cell> _playingObjects = new Dictionary<Vector3Int, Cell>();

    void Start()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        GlobalEventManager.AddFilledCell.AddListener(AddCell);
        GlobalEventManager.RemoveFilledCell.AddListener(RemoveCell);
        GlobalEventManager.SendPlaingArea.AddListener(AddPlayArea);
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

    private void Update()
    {
        Debug.Log(_mainCamera.ScreenToWorldPoint(Input.mousePosition));
        // Debug.Log();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isDrag)
        {
            var clickWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

            var clickCellPosition = _playArea.WorldToCell(clickWorldPosition);
            
            Debug.Log($"Click on {clickCellPosition}, world position = {clickWorldPosition}");

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
