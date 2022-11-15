using System;
using System.Collections;
using System.Collections.Generic;
using Clicker;
using Unity.Mathematics;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _coin;
    [SerializeField] private Transform _target;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public void SpawnCoin(Vector3 coinPosition)
    {
        var ncp = Camera.main.WorldToScreenPoint(coinPosition);
        var c = Instantiate(_coin, ncp, quaternion.identity,_canvas.transform);
        c.GetComponent<Coin>().Initialize(_target.position);
    }
}
