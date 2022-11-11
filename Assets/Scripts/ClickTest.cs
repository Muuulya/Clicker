using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ClickTest : MonoBehaviour
{

    private Tilemap map;
    private Camera mainCamera;
    
    
    void Start()
    {
        map = GetComponent<Tilemap>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var clickWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var worldToCell = map.WorldToCell(clickWorldPosition);
            Debug.Log(worldToCell);
        }
    }
}
