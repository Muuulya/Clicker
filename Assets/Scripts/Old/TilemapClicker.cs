// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.Tilemaps;
//
// public class TilemapClicker : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
// {
//
//     public Tilemap tilemapPlayingArea;
//     public Tilemap tilemapPlayingElements;
//
//     public TileBase[] tiles0;
//     public TileBase[] tiles1;
//
//     public List<TileBase> tileList0;
//     public List<TileBase> tileList1;
//
//     private Camera mainCamera;
//     private bool drag = false;
//
//     // Start is called before the first frame update
//     void Start()
//     {
//         mainCamera = Camera.main;
//         // Gggg();
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         // if (Input.GetMouseButton(0))
//         // {
//         //     var clickWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
//         //
//         //     var clickCellPosition = tilemap0.WorldToCell(clickWorldPosition);
//         //     
//         //     Debug.Log(clickCellPosition);
//         //     
//         //     tilemap1.SetTile(clickCellPosition, tiles1[0]);
//         // }
//     }
//
//     public void Gggg()
//     {
//         var x = tilemapPlayingArea.GetTilesBlock(tilemapPlayingArea.cellBounds);
//         
//         Debug.Log(x.Length);
//         // for (int i = 0; i < x.Length; i++)
//         // {
//         //     tilemap0.SetTile(x[i],);
//         // }
//         // tilemap0.SetTilesBlock();
//
//         tilemapPlayingArea.CompressBounds();
//
//         var y = tilemapPlayingArea.GetTilesBlock(tilemapPlayingArea.cellBounds);
//         
//         Debug.Log(y.Length);
//
//         var z = tilemapPlayingArea.cellBounds;
//         var min = z.min;
//         var max = z.max;
//         
//         // tilemapPlayingArea.SetTile(min,tiles1[0]);
//         // tilemapPlayingArea.SetTile(max,tiles1[1]);
//
//         var cells = new List<Vector3Int>();
//         for (int i = min.y; i < max.y; i++)
//         {
//             for (int j = min.x; j < max.x; j++)
//             {
//                 if (tilemapPlayingArea.GetTile(new Vector3Int(j,i,0)) != null)
//                 {
//                     cells.Add(new Vector3Int(j,i,0));
//                     
//                 }
//             }
//         }
//         
//         Debug.Log($"Cells count = {cells.Count}");
//         
//         StartCoroutine(CellsFild(cells));
//         
//         
//         IEnumerator CellsFild(List<Vector3Int> cells)
//         {
//             var delay = new WaitForSeconds(0.2f);
//             foreach (var cell in cells)
//             {
//                 tilemapPlayingArea.SetTile(cell,tiles1[1]);
//                 yield return delay;
//             }
//         }
//         
//         // var x = tilemap0.cellLayout;
//         // Debug.Log(x);
//         
//         // BoundsInt a = new BoundsInt();
//         // var min = x.min;
//         // var max = x.max;
//         Debug.Log($"min = {min}, max = {max}");
//         // tilemap0.SetTile(min,tiles1[0]);
//         // tilemap0.SetTile(max,tiles1[1]);
//     }
//
//     public void OnPointerClick(PointerEventData eventData)
//     {
//         if (!drag)
//         {
//             Debug.Log("Click");
//         
//             var clickWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
//
//             var clickCellPosition = tilemapPlayingArea.WorldToCell(clickWorldPosition);
//             
//             Debug.Log(clickCellPosition);
//             
//             tilemapPlayingElements.SetTile(clickCellPosition, tiles1[0]);
//         }
//     }
//
//     public void OnDrag(PointerEventData eventData)
//     {
//         Debug.Log("OnDrag");
//     }
//
//     public void OnEndDrag(PointerEventData eventData)
//     {
//         Debug.Log("OnEndDrag");
//         drag = false;
//     }
//
//     public void OnBeginDrag(PointerEventData eventData)
//     {
//         Debug.Log("OnBeginDrag");
//         drag = true;
//     }
// }






using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

// using Random = Unity.Mathematics.Random;

// using Random = System.Random;

public partial class TilemapClicker : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private Tilemap _tilemapBackground;
    [SerializeField] private Tilemap _tilemapPlayArea;
    [SerializeField] private Tilemap _tilemapPlayingElements;
    [SerializeField] private List<TileBase> _playingElements;

    [SerializeField] private int _maxFilledCells;
    [SerializeField] private float _spawnDelay;

    [SerializeField] private GameObject _spritePrefab;

    private Camera _mainCamera;
    private Dictionary<Vector3Int, CellsStatus> _cells;
    private Unity.Mathematics.Random _random;
    private float _lastSpawnTime = 0;
    private bool drag = false;

    enum CellsStatus
    {
        Blank,
        Empty,
        Filled
    }
    void Start()
    {
        _random = new Unity.Mathematics.Random(Convert.ToUInt32(DateTime.Now.Millisecond));
        _mainCamera = Camera.main;
        _tilemapBackground.CompressBounds();
        _tilemapPlayArea.CompressBounds();
        _tilemapPlayingElements.origin = _tilemapPlayArea.origin;
        _tilemapPlayingElements.size = _tilemapPlayArea.size;

        var minPosition = _tilemapPlayArea.cellBounds.min;
        var maxPosition = _tilemapPlayArea.cellBounds.max;

        _cells = new Dictionary<Vector3Int, CellsStatus>();

        for (int i = minPosition.x; i < maxPosition.x; i++)
        {
            for (int j = minPosition.y; j < maxPosition.y; j++)
            {
                if (_tilemapPlayArea.GetTile(new Vector3Int(i, j, 0)) != null)
                {
                    _cells.Add(new Vector3Int(i, j, 0), CellsStatus.Blank);
                }
                else
                {
                    _cells.Add(new Vector3Int(i, j, 0), CellsStatus.Empty);
                }
            }
        }
        
        Debug.Log($"Backgroun length = {_tilemapBackground.GetTilesBlock(_tilemapBackground.cellBounds).Length}"); 
        Debug.Log($"PlayArea length = {_tilemapPlayArea.GetTilesBlock(_tilemapPlayArea.cellBounds).Length}");
        Debug.Log($"PlayingElements length = {_tilemapPlayingElements.GetTilesBlock(_tilemapPlayingElements.cellBounds).Length}"); 
        // Debug.Log($"Backgroun length = {_tilemapBackground.GetTilesBlock(_tilemapBackground.cellBounds).Length}");

    }

    private void Update()
    {
        if (Time.time > _lastSpawnTime + _spawnDelay)
        {
            int filledCellCount = (from cell in _cells
                where cell.Value == CellsStatus.Filled
                select cell).ToList().Count;
            // Debug.Log($"filledCellCount = {filledCellCount}, maxFilledCells = {_maxFilledCells}");
            if (filledCellCount < _maxFilledCells)
            {
                var blankCells = (from cell in _cells
                        where cell.Value == CellsStatus.Blank
                        select cell)
                    .ToDictionary(cell => cell.Key, cell => cell.Value);
                if (blankCells.Count > 0)
                {
                    var pos = blankCells.ElementAt(_random.NextInt(0, blankCells.Count)).Key;
                    _tilemapPlayingElements.SetTile(pos,_playingElements[0]);
                    _cells[pos] = CellsStatus.Filled;
                }
            }

            _lastSpawnTime = Time.time;
        }
    }

    private GameObject currentSprite;
    public GameObject parant;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!drag)
        {
            var clickWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

            var clickCellPosition = _tilemapPlayArea.WorldToCell(clickWorldPosition);
            
            Debug.Log($"Click on {clickCellPosition}");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentSprite.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(currentSprite);
        drag = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Start");
        drag = true;
        var clickWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        var clickCellPosition = _tilemapPlayArea.WorldToCell(clickWorldPosition);
                    currentSprite = Instantiate(_spritePrefab, Vector3.zero, Quaternion.identity, parant.transform);
                    currentSprite.GetComponent<Image>().sprite = _tilemapPlayingElements.GetSprite(clickCellPosition);

        
        if (_tilemapPlayingElements.GetTile(clickCellPosition) != null)
        {
            Debug.Log("Inst");
        }
    }
}