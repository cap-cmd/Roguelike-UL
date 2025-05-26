using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;
        public CellObject ContainedObject;
    }

    [SerializeField] private int _boardWidth;
    [SerializeField] private int _boardHeight;
    [SerializeField] private Tile[] _groundTiles;
    [SerializeField] private Tile[] _wallTiles;
    [SerializeField] private FoodObject[] foodPrefabs;
    [SerializeField] private int minFoodCount;
    [SerializeField] private int maxFoodCount;

    private Tilemap _tilemap;
    private Grid _grid;
    private CellData[,] _boardData;
    private List<Vector2Int> _emptyCellList;

    private void Awake()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
        _grid = GetComponent<Grid>();
    }

    public void Init()
    {
        _boardData = new CellData[_boardWidth, _boardHeight];
        _emptyCellList = new();

        for (int i = 0; i < _boardWidth; i++)
        {
            for (int j = 0; j < _boardHeight; j++)
            {
                Tile tile;
                _boardData[i, j] = new CellData();

                if (i == 0 || j == 0 || i == _boardWidth - 1 || j == _boardHeight - 1)
                {
                    tile = _wallTiles[Random.Range(0, _wallTiles.Length)];
                    _boardData[i, j].Passable = false;
                }
                else
                {
                    tile = _groundTiles[Random.Range(0, _groundTiles.Length)];
                    _boardData[i, j].Passable = true;
                    _emptyCellList.Add(new Vector2Int(i, j));
                }

                _tilemap.SetTile(new Vector3Int(i, j, 0), tile);
            }
        }
        _emptyCellList.Remove(new Vector2Int(1, 1));
        GenetateFood();
    }

    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return _grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= _boardWidth || cellIndex.y < 0 || cellIndex.y >= _boardHeight)
        {
            return null;
        }
        return _boardData[cellIndex.x, cellIndex.y];
    }

    private void GenetateFood()
    {
        int foodCount = Random.Range(minFoodCount, maxFoodCount + 1);

        for (int i = 0; i < foodCount; i++)
        {
            int randomCellIndex = Random.Range(0, _emptyCellList.Count);
            int randomFoodIndex = Random.Range(0, foodPrefabs.Length);
            Vector2Int coord = _emptyCellList[randomCellIndex];

            CellData data = _boardData[coord.x, coord.y];
            FoodObject newFood = Instantiate(foodPrefabs[randomFoodIndex]);
            newFood.transform.position = CellToWorld(coord);
            data.ContainedObject = newFood;
        }
    }
}