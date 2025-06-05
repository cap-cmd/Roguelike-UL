using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private bool _isMoving;
    private Vector3 _moveTarget;

    private BoardManager _boardManager;
    private Vector2Int _cellPosition;
    private bool _isGameOver;
    private float _inputBlockDuration = 0;
    private float _inputLockTime = 0.1f;

    public Animator Animator { get; private set; }
    public Vector2Int CellPosition { get => _cellPosition; }


    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Init();
    }

    private void Update()
    {
        Vector2Int newCellTarget = _cellPosition;
        bool hasMoved = false;
        _inputBlockDuration -= Time.deltaTime;

        if (_inputBlockDuration <= 0)
        {
            if (_isGameOver)
            {
                if (Keyboard.current.enterKey.wasPressedThisFrame)
                {
                    GameManager.Instance.StartNewGame();
                    _isGameOver = false;
                    _inputBlockDuration = _inputLockTime;
                }
                return;
            }

            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                newCellTarget.y += 1;
                hasMoved = true;
                _inputBlockDuration = _inputLockTime;
            }
            else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                newCellTarget.y -= 1;
                hasMoved = true;
                _inputBlockDuration = _inputLockTime;
            }
            else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                newCellTarget.x -= 1;
                hasMoved = true;
                _inputBlockDuration = _inputLockTime;
            }
            else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                newCellTarget.x += 1;
                hasMoved = true;
                _inputBlockDuration = _inputLockTime;
            }
        }

        if (_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _moveTarget, _moveSpeed * Time.deltaTime);

            if (transform.position == _moveTarget)
            {
                _isMoving = false;
                var cellData = _boardManager.GetCellData(_cellPosition);

                if (cellData.ContainedObject != null)
                {
                    cellData.ContainedObject.PlayerEntered();
                }
            }
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
                }
                GameManager.Instance.TurnManager.Tick();
            }
        }
    }

    public void Init()
    {
        _isGameOver = false;
        _isMoving = false;
    }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        _boardManager = boardManager;
        _cellPosition = cell;

        Vector3 coord = _boardManager.CellToWorld(_cellPosition);
        transform.position = coord;
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    private void MoveTo(Vector2Int cell)
    {
        _cellPosition = cell;

        _isMoving = true;
        _moveTarget = _boardManager.CellToWorld(_cellPosition);
    }
}
