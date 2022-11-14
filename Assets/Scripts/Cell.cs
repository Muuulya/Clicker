using UnityEngine;

namespace Clicker
{
    public class Cell
    {
        public Vector3Int Position { get; }
        public CellStatus CellStatus { get; set; }

        public Cell(Vector3Int position)
        {
            Position = position;
            CellStatus = CellStatus.Empty;
        }

        public Cell(Cell cell)
        {
            Position = cell.Position;
            CellStatus = cell.CellStatus;
        }
    }
}