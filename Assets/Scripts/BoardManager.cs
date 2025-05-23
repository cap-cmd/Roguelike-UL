using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;
    }

    [SerializeField] private PlayerController _player;
    [SerializeField] private int _boardWidth;
    [SerializeField] private int _boardHeight;
    [SerializeField] private Tile[] _groundTiles;
    [SerializeField] private Tile[] _wallTiles;

    private Tilemap _tilemap;
    private Grid _grid;
    private CellData[,] _boardData;

    private void Awake()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
        _grid = GetComponent<Grid>();
    }

    private void Start()
    {
        _boardData = new CellData[_boardWidth, _boardHeight];

        for (int i = 0; i < _boardWidth; i++)
        {
            for (int j = 0; j < _boardHeight; j++)
            {
                Tile tile;
                _boardData[i, j] = new CellData();

                if (i == 0 || j == 0 || i == _boardWidth - 1 ||  j == _boardHeight - 1)
                {
                    tile = _wallTiles[Random.Range(0, _wallTiles.Length)];
                    _boardData[i,j].Passable = false;
                }
                else
                {
                    tile = _groundTiles[Random.Range(0, _groundTiles.Length)];
                    _boardData[i,j].Passable = true;
                }

                _tilemap.SetTile(new Vector3Int(i,j,0), tile);  
            }
        }

        _player.Spawn(this, new Vector2Int(1, 1));
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
}