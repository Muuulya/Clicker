using System;
using System.Collections;
using System.Collections.Generic;
using Clicker;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _coin;
    [SerializeField] private Transform _target;

    private Camera _mainCamera;
    private Vector3 _offsetToCentrCell = new Vector3(0.5f, 0.5f, 0);
    private Random _random;

    private void Start()
    {
        _mainCamera = Camera.main;
        _random = new Random(Convert.ToUInt32(DateTime.Now.Millisecond));
    }

    public void SpawnCoin(Vector3 coinPosition, int quantity)
    {
        var screenCoinPosition = _mainCamera.WorldToScreenPoint(coinPosition + _offsetToCentrCell);

        for (int i = 0; i < quantity; i++)
        {
            var randomOffset = _random.NextFloat2(-50, 50);
            var pos = screenCoinPosition + new Vector3(randomOffset.x, randomOffset.y, 0);
            var coin = Instantiate(_coin, pos, quaternion.identity,_canvas.transform);
            coin.GetComponent<Coin>().Initialize(_target.position);
        }
    }
}
