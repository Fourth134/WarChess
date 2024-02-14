using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private ChessTile _tilePrefad;

    [SerializeField] private Transform _cam;
    [SerializeField] private Transform _camcontrol;

    private Dictionary<Vector2, ChessTile> _tiles;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, ChessTile>();
        for (int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefad, new Vector3(x, 0, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (y % 2 == 0 && x % 2 != 0);
                spawnedTile.Init(isOffset);

                _tiles[new Vector2(x,y)] = spawnedTile;

            }
        }

        _camcontrol.transform.position = new Vector3((float)_width/2 - 0.5f,10,(float)_height / 2 - 0.5f - 10);
        _cam.transform.Rotate(Vector3.right,45);
    }

    public ChessTile GetChessTileAtPosition(Vector2 pos)//通过给定坐标，返回特定的ChessTile对象。
    {
        if(_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }
}
