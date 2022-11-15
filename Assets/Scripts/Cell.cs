using UnityEngine;

namespace Clicker
{
    public class Cell
    {
        public Vector3Int Position { get; }
        public CellStatus CellStatus { get; set; }
        
        public Vector3 WorldPosition { get; }

        public Cell(Vector3Int position, Vector3 worldPosition)
        {
            Position = position;
            WorldPosition = worldPosition;
            CellStatus = CellStatus.Empty;
        }

        public Cell(Cell cell)
        {
            Position = cell.Position;
            CellStatus = cell.CellStatus;
        }
    }
}