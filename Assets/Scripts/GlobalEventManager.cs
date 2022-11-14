using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Random = Unity.Mathematics.Random;

namespace Clicker
{
    public class GlobalEventManager
    {
        public static UnityEvent<Dictionary<Vector3Int, Cell>, Random, Tilemap, TileBase> LoadLevel = 
            new UnityEvent<Dictionary<Vector3Int, Cell>, Random, Tilemap, TileBase>();
        
        public static UnityEvent<Vector3Int, Cell> AddFilledCell =
                new UnityEvent<Vector3Int, Cell>();
        public static UnityEvent<Vector3Int> RemoveFilledCell = 
            new UnityEvent<Vector3Int>();

        public static UnityEvent<Earnings> SendEarnings =
                new UnityEvent<Earnings>();

        public static UnityEvent<Tilemap> SendPlaingArea =
            new UnityEvent<Tilemap>();

        public static UnityEvent<Tilemap> SendPlaingElements =
            new UnityEvent<Tilemap>();

        public static UnityEvent<Cell> AccrueMoney =
            new UnityEvent<Cell>();
        
        
    }
    
    
}