using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Random = Unity.Mathematics.Random;

namespace Clicker
{
    public class GlobalEventManager
    {
        public static UnityEvent<Earnings> SendEarnings =
                new UnityEvent<Earnings>();
        
        public static UnityEvent<Cell> AccrueMoney =
            new UnityEvent<Cell>();
    }
    
    
}