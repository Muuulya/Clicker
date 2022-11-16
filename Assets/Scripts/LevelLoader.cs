using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Clicker;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private TilemapsData _tilemapsData;
    [SerializeField] private Tilemap _tilemapPlayArea;
    // [SerializeField] private Tilemap _tilemapPlayingElements;
    [SerializeField] private Earnings _earnings;
    
    private string _savePath = "/MySaveData.dat";
    
    void Start()
    {
        if (!File.Exists(Application.persistentDataPath + _savePath))
        {
            StartInitialize();
        }
    }

    private void StartInitialize()
    {
        _tilemapPlayArea.CompressBounds();
        var minPosition = _tilemapPlayArea.cellBounds.min;
        var maxPosition = _tilemapPlayArea.cellBounds.max;
        
        var cells = new Dictionary<Vector3Int, Cell>();
        
        for (int i = minPosition.x; i < maxPosition.x; i++)
        {
            for (int j = minPosition.y; j < maxPosition.y; j++)
            {
                var pos = new Vector3Int(i, j, 0);
                if (_tilemapPlayArea.GetTile(pos) != null)
                {
                    var worldPos = _tilemapPlayArea.CellToWorld(pos);
                    var cell = new Cell(pos, worldPos);
                    cells.Add(pos, cell);
                }
            }
        }
        _tilemapsData.Initialize(cells);
        _earnings.Initialize(0);
    }

    private void OnApplicationPause(bool hasFocus)
    {
        if (hasFocus)
        {
            SaveGame();
        }
        else if(File.Exists(Application.persistentDataPath + _savePath))
        {
            LoadGame();
        }
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter(); 
        FileStream file = File.Create(Application.persistentDataPath + _savePath); 
        SaveData data = new SaveData();

        data.Cells = SaveCells(_tilemapsData.GetAllCells());
        data.Money = _earnings.Money;
        data.DateTime = DateTime.Now;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }

    private void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + _savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + _savePath, FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            
            _tilemapsData.Initialize(LoadCells(data.Cells));
            _earnings.Initialize(data.Money);
            
            var time = DateTime.Now - data.DateTime;
            var totalSeconds = time.Seconds + ((time.Minutes + ((time.Hours + (time.Days * 24)) * 60)) * 60);
            _earnings.AccrueCoinsForPastPeriod(totalSeconds);
            
            file.Close();
            Debug.Log("Game data loaded!");
        }
        else
            Debug.LogError("There is no save data!");
    }

   
    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + _savePath))
        {
            File.Delete(Application.persistentDataPath + _savePath);
            StartInitialize();
            Debug.Log("Data reset complete!");
        }
        else
            Debug.LogError("No save data to delete.");
    }

    private Dictionary<string, CellStatus> SaveCells(Dictionary<Vector3Int,Cell> cells)
    {
        var result = new Dictionary<string, CellStatus>();
        foreach (var cell in cells)
        {
            string s = $"{cell.Key.x} {cell.Key.y} {cell.Key.z}";
            result.Add(s,cell.Value.CellStatus);
        }
        return result;
    }

    private Dictionary<Vector3Int, Cell> LoadCells(Dictionary<string, CellStatus> cells)
    {
        var result = new Dictionary<Vector3Int, Cell>();
        foreach (var cell in cells)
        {
            var s = cell.Key.Split(' ');
            var x = Int32.Parse(s[0]);
            var y = Int32.Parse(s[1]);
            var z = Int32.Parse(s[2]);
            var pos = new Vector3Int(x, y, z);
            var worldPos = _tilemapPlayArea.CellToWorld(pos);
            result.Add(pos, new Cell(pos, worldPos, cell.Value));
        }
        return result;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
