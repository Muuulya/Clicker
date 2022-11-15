using System;
using UnityEngine;

namespace Clicker
{
    public class Coin : MonoBehaviour
    {
        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private float time = 0;
        
        public void Initialize(Vector3 target)
        {
            _targetPosition = target;
            _startPosition = gameObject.transform.position;
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, time);

            time += Time.deltaTime;
        }
    }
}