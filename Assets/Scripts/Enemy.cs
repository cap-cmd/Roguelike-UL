using System;
using UnityEngine;

public class Enemy : CellObject
{
    [SerializeField] private int health = 3;
    [SerializeField] private int moveSpeed = 5;
    private Animator _animator;

    private bool _hasMoved;
    private bool _isMoving;
    private Vector3 moveTarget;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GameManager.Instance.TurnManager.OnTick += Move;
    }

    private void OnDisable()
    {
        GameManager.Instance.TurnManager.OnTick -= Move;
    }

    private void Update()
    {
        if (_isMoving)
        {
            Moving();
        }

        if (_hasMoved)
        {
        }
    }

    public override bool PlayerWantsToEnter()
    {
        health -= 1;
        GameManager.Instance.Player.Animator.SetTrigger("Attack");

        if (health <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    private void Move()
    {
        var distance = GameManager.Instance.Player.CellPosition - m_Cell;
        var absDistance = new Vector2Int(Math.Abs(distance.x), Math.Abs(distance.y));
        var direction = new Vector2Int(Math.Sign(distance.x), Math.Sign(distance.y));

        var moveX = new Vector2Int(direction.x, 0);
        var moveY = new Vector2Int(0, direction.y);

        var primaryTarget = absDistance.x >= absDistance.y ? moveX : moveY;
        var secondaryTarget = absDistance.x >= absDistance.y ? moveY : moveX;

        bool canAttackPlayer = (absDistance.x == 0 && absDistance.y == 1) || (absDistance.x == 1 && absDistance.y == 0);

        if (canAttackPlayer)
        {
            Attack();
            return;
        }

        TryMoveInPreferredDirection(primaryTarget, secondaryTarget);
    }

    private void Attack()
    {
        _animator.SetTrigger("Attack");
        GameManager.Instance.ChangeFood(-2);
    }

    private void TryMoveInPreferredDirection(Vector2Int primaryTarget, Vector2Int secondaryTarget)
    {
        _ = TryMoveInDirection(primaryTarget) || TryMoveInDirection(secondaryTarget);
    }

    private bool TryMoveInDirection(Vector2Int direction)
    {
        var newCellPosition = m_Cell + direction;
        var newCellData = GameManager.Instance.BoardManager.GetCellData(newCellPosition);

        var currentCellData = GameManager.Instance.BoardManager.GetCellData(m_Cell);

        if (newCellData.ContainedObject == null)
        {
            m_Cell = newCellPosition;

            currentCellData.ContainedObject = null;
            newCellData.ContainedObject = this;

            moveTarget = GameManager.Instance.BoardManager.CellToWorld(m_Cell);
            _isMoving = true;

            return true;
        }

        return false;
    }

    private void Moving()
    {
        _animator.SetBool("Moving", true);
        transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);

        if (transform.position == moveTarget)
        {
            _animator.SetBool("Moving", false);
            _isMoving = false;
        }
    }
}
