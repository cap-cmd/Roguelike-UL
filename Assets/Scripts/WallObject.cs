using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    [SerializeField] private Tile[] ObstacleTiles;
    [SerializeField] private Tile[] DestroyObstacleTile;
    [SerializeField] private int hitpoint = 3;


    private Tile _clearTile;
    private int _tileIndex;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        _clearTile = GameManager.Instance.BoardManager.GetCellTile(cell);

        _tileIndex = Random.Range(0, ObstacleTiles.Length);
        GameManager.Instance.BoardManager.SetCellTile(cell, ObstacleTiles[_tileIndex]);
    }

    public override bool PlayerWantsToEnter()
    {
        hitpoint -= 1;
        if (hitpoint == 1)
        {
            GameManager.Instance.BoardManager.SetCellTile(m_Cell, DestroyObstacleTile[_tileIndex]);
            return false;
        }
        else if (hitpoint <= 0)
        {
            GameManager.Instance.BoardManager.SetCellTile(m_Cell, _clearTile);
            Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }
}
