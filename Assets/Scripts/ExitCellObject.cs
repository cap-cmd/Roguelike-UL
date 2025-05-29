using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitCellObject : CellObject
{
    [SerializeField] private Tile exitTile;

    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
        GameManager.Instance.BoardManager.SetCellTile(coord, exitTile);
    }

    public override void PlayerEntered()
    {
        GameManager.Instance.BoardManager.CleanLevel();
        GameManager.Instance.NewLevel();
    }
}
