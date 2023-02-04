using System;
using UnityEngine;

public enum Direction
{
    None,
    Up,
    Left,
    Down,
    Right,
}

public class CharacterMotor : MonoBehaviour
{
    public event Action<Direction> OnDirectionChanged;
    public event Action OnAlignedWithGrid;
    public event Action OnResetPosition;
    public event Action OnDisabled;
    public event Action<float> OnSpeedSpecial;


    public float MoveSpeed;
    public LayerMask CollisionLayerMask
    {
        get => _collisionLayerMask;
    }

    private Vector2 _desiredMovementDirection;
    private Vector2 _currentMovementDirection;

    private Rigidbody2D _rigidbody;

    private Vector2 _boxSize;

    private LayerMask _collisionLayerMask;

    private Vector3 _initialPosition;

    public float NaturalSpeed
    { get => _naturalSpeed; }
    private float _naturalSpeed;

    private bool gizmosOn = false;

    public Direction CurrentMoveDirection
    {
        get
        {
            //up
            if (_currentMovementDirection.y > 0)
            {
                return Direction.Up;
            }
            //left
            if (_currentMovementDirection.x < 0)
            {
                return Direction.Left;

            }
            //down
            if (_currentMovementDirection.y < 0)
            {
                return Direction.Down;

            }
            //right
            if (_currentMovementDirection.x > 0)
            {
                return Direction.Right;

            }

            return Direction.None;
        }
    }

    public void SetMotorSpeed(float hyperSpeed, float duration)
    {
        MoveSpeed = hyperSpeed;
        OnSpeedSpecial?.Invoke(duration);
    }
    public void SetMoveDirection(Direction newMoveDirection)
    {
        switch (newMoveDirection)
        {
            default:
            case Direction.None:
                break;
            case Direction.Up:
                _desiredMovementDirection = Vector2.up;
                break;
            case Direction.Left:
                _desiredMovementDirection = Vector2.left;
                break;
            case Direction.Down:
                _desiredMovementDirection = Vector2.down;
                break;
            case Direction.Right:
                _desiredMovementDirection = Vector2.right;
                break;
        }
    }

    public void ResetPosition()
    {
        _desiredMovementDirection = Vector2.zero;
        _currentMovementDirection = Vector2.zero;
        transform.position = _initialPosition;
        OnResetPosition?.Invoke();
    }

    public void CollideWithGates(bool shouldCollide)
    {
        if (shouldCollide)
        {
            _collisionLayerMask = LayerMask.GetMask(new string[] { "Level", "Gates" });
        }
        else
        {
            _collisionLayerMask = LayerMask.GetMask("Level");

        }
    }

    private void Start()
    {
        gizmosOn = true;
        _desiredMovementDirection = Vector2.zero;
        _currentMovementDirection = Vector2.zero;
        CollideWithGates(true);
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxSize = GetComponent<BoxCollider2D>().size;

        _initialPosition = transform.position;
        _naturalSpeed = MoveSpeed;
    }

    private void FixedUpdate()
    {
        float moveDistance = MoveSpeed * Time.fixedDeltaTime;
        var nextMovePosition = _rigidbody.position + _currentMovementDirection * moveDistance;

        //up
        if (_currentMovementDirection.y > 0)
        {
            var maxY = Mathf.CeilToInt(_rigidbody.position.y); //ceil = o teto de um número || 1.1 -> 2

            if (nextMovePosition.y >= maxY)
            {
                transform.position = new Vector2(_rigidbody.position.x, maxY);
                moveDistance = nextMovePosition.y - maxY;
            }
        }
        //left
        if (_currentMovementDirection.x < 0)
        {
            var minX = Mathf.FloorToInt(_rigidbody.position.x); //floor = o chao de um número || 1.1 -> 1

            if (nextMovePosition.x <= minX)
            {
                transform.position = new Vector2(minX, _rigidbody.position.y);
                moveDistance = minX - nextMovePosition.x;
            }
        }
        //down
        if (_currentMovementDirection.y < 0)
        {
            var minY = Mathf.FloorToInt(_rigidbody.position.y); //floor = o chao de um número || 1.1 -> 1

            if (nextMovePosition.y <= minY)
            {
                transform.position = new Vector2(_rigidbody.position.x, minY);
                moveDistance = minY - nextMovePosition.y;
            }
        }
        //right
        if (_currentMovementDirection.x > 0)
        {
            var maxX = Mathf.CeilToInt(_rigidbody.position.x); //ceil = o teto de um número || 1.1 -> 2

            if (nextMovePosition.x >= maxX)
            {
                transform.position = new Vector2(maxX, _rigidbody.position.y);
                moveDistance = nextMovePosition.x - maxX;
            }
        }

        Physics2D.SyncTransforms(); //atualiza o rigidbody para ficar com a mesma posição do transform que alteramos ali em cima

        //verifica alinhamento 
        if (_rigidbody.position.x == Mathf.CeilToInt(_rigidbody.position.x) && _rigidbody.position.y == Mathf.CeilToInt(_rigidbody.position.y) || _currentMovementDirection == Vector2.zero)
        {
            OnAlignedWithGrid?.Invoke();

            if (_currentMovementDirection != _desiredMovementDirection)
            {
                //bit shift operation
                //00100000 = 1024 << 1 -> está pegando o "1" do bit e movendo um pra esquerda
                //Assim que a layer funciona, por bits(32)
                if (!Physics2D.BoxCast(_rigidbody.position, _boxSize, 0, _desiredMovementDirection, 1f, CollisionLayerMask))
                {
                    _currentMovementDirection = _desiredMovementDirection;
                    OnDirectionChanged?.Invoke(CurrentMoveDirection);
                }
            }

            if (Physics2D.BoxCast(_rigidbody.position, _boxSize, 0, _currentMovementDirection, 1f, CollisionLayerMask))
            {
                _currentMovementDirection = Vector2.zero;
                OnDirectionChanged?.Invoke(CurrentMoveDirection);
            }


        }

        _rigidbody.MovePosition(_rigidbody.position + _currentMovementDirection * moveDistance);

    }

    private void OnDisable()
    {
        OnDisabled?.Invoke();
    }

    private void OnDrawGizmos()
    {
        if (!gizmosOn)
            return;

        Gizmos.color = Color.magenta;
        if (Physics2D.BoxCast(_rigidbody.position, _boxSize, 0, _desiredMovementDirection, 1f, 1 << LayerMask.NameToLayer("Level")))
            Gizmos.DrawLine(transform.position, Physics2D.BoxCast(_rigidbody.position, _boxSize, 0, _desiredMovementDirection, 1f, 1 << LayerMask.NameToLayer("Level")).point);
    }
}
