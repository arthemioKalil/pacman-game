using System;
using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class GhostMove : MonoBehaviour
{
    public event Action OnUpdateMoveTarget;

    public CharacterMotor CharacterMotor { get => _motor; }

    private CharacterMotor _motor;
    private Vector2 _boxSize;
    private Vector2 _targetMoveLocation;
    private bool _allowReverseDirection;

    public void AllowReverseDirection()
    {
        _allowReverseDirection = true;
    }


    public void SetTargetMoveLocation(Vector2 targetMoveLocation)
    {
        _targetMoveLocation = targetMoveLocation;
    }
    void Start()
    {
        _motor = GetComponent<CharacterMotor>();
        _boxSize = GetComponent<BoxCollider2D>().size;

        _motor.OnAlignedWithGrid += CharacterMotor_OnAlignedWithGrid;

        CharacterMotor_OnAlignedWithGrid();

        _allowReverseDirection = false;
    }

    private void CharacterMotor_OnAlignedWithGrid()
    {
        OnUpdateMoveTarget?.Invoke();
        ChangeDirection();
    }

    private void ChangeDirection()
    {

        var closestDistance = float.MaxValue;
        Direction finalDirection = Direction.None;

        UpdateFinalDirection(Direction.Up, Vector3.up, ref closestDistance, ref finalDirection);
        UpdateFinalDirection(Direction.Left, Vector3.left, ref closestDistance, ref finalDirection);
        UpdateFinalDirection(Direction.Down, Vector3.down, ref closestDistance, ref finalDirection);
        UpdateFinalDirection(Direction.Right, Vector3.right, ref closestDistance, ref finalDirection);



        _motor.SetMoveDirection(finalDirection);

        _allowReverseDirection = false;
    }

    private void UpdateFinalDirection(Direction direction, Vector3 offSet, ref float closestDistance, ref Direction finalDirection)
    {
        if (CheckIfDirectionMovable(direction))
        {
            var dist = Vector2.Distance(transform.position + offSet, _targetMoveLocation);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                finalDirection = direction;
            }
        }
    }

    private bool CheckIfDirectionMovable(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return !Physics2D.BoxCast(transform.position, _boxSize, 0, Vector2.up, 1f, _motor.CollisionLayerMask) && (_motor.CurrentMoveDirection != Direction.Down || _allowReverseDirection);

            case Direction.Left:
                return !Physics2D.BoxCast(transform.position, _boxSize, 0, Vector2.left, 1f, _motor.CollisionLayerMask) && (_motor.CurrentMoveDirection != Direction.Right || _allowReverseDirection);

            case Direction.Down:
                return !Physics2D.BoxCast(transform.position, _boxSize, 0, Vector2.down, 1f, _motor.CollisionLayerMask) && (_motor.CurrentMoveDirection != Direction.Up || _allowReverseDirection);

            case Direction.Right:
                return !Physics2D.BoxCast(transform.position, _boxSize, 0, Vector2.right, 1f, _motor.CollisionLayerMask) && (_motor.CurrentMoveDirection != Direction.Left || _allowReverseDirection);
        }

        return false;
    }
}