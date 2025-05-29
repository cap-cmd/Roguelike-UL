using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager _boardManager;
    private Vector2Int _cellPosition;
    private bool _isGameOver = false;

    private void Update()
    {
        if (_isGameOver)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                GameManager.Instance.StartNewGame();
                _isGameOver = false;
            }
            return;
        }

        Vector2Int newCellTarget = _cellPosition;
        bool hasMoved = false;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }

        if (hasMoved)
        {
            BoardManager.CellData cellData = _boardManager.GetCellData(newCellTarget);

            if (cellData != null && cellData.Passable)
            {
                if (cellData.ContainedObject == null)
                {
                    MoveTo(newCellTarget);
                }
                else if (cellData.ContainedObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget);
                    cellData.ContainedObject.PlayerEntered();
                }
                GameManager.Instance.TurnManager.Tick();
            }
        }
    }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        _boardManager = boardManager;
        MoveTo(cell);
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    private void MoveTo(Vector2Int cell)
    {
        _cellPosition = cell;
        transform.position = _boardManager.CellToWorld(_cellPosition);
    }
}
