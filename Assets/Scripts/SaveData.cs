using System;
using System.Collections;
using System.Collections.Generic;
using Clicker;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class SaveData
{
    public Dictionary<string, CellStatus> Cells;
    public int Money;
    public DateTime DateTime;
    public int PurchasedCells;
    public int CurrentCellPrice;
}
