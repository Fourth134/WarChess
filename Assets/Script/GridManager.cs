using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] public int _width, _height;

    [SerializeField] private ChessTile _tilePrefad;

    [SerializeField] private Transform _cam;
    [SerializeField] private Transform _camcontrol;
    [SerializeField] private float tileSize = 5f; // ���̸��Ӵ�С

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
            for (int y = 0; y < _height; y++)
            {
                // �����µĸ���λ��
                Vector3 newPosition = new Vector3(x * tileSize, 0, y * tileSize);

                var spawnedTile = Instantiate(_tilePrefad, newPosition, Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (y % 2 == 0 && x % 2 != 0);
                spawnedTile.Init(isOffset);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        // �����������λ�ú���ת
        _camcontrol.transform.position = new Vector3(-5, 10, -5);
        _cam.transform.Rotate(Vector3.right, 40);
        _camcontrol.transform.Rotate(Vector3.up, 40);
    }


    public ChessTile GetChessTileAtPosition(Vector2 pos)//ͨ���������꣬�����ض���ChessTile����
    {
        if(_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }
}
