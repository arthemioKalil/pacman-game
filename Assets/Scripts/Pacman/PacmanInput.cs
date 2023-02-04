using UnityEngine;

[RequireComponent(typeof(CharacterMotor))] //Essa classe requer ter a classe CharacterMotor
public class PacmanInput : MonoBehaviour, IMovableCharacter
{
    private CharacterMotor _motor;

    public void ResetPosition()
    {
        _motor.ResetPosition();
    }

    public void StartMoving()
    {
        _motor.enabled = true;
    }

    public void StopMoving()
    {
        _motor.enabled = false;

    }

    void Start()
    {
        _motor = GetComponent<CharacterMotor>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            _motor.SetMoveDirection(Direction.Up);

        else if (Input.GetKeyDown(KeyCode.A))
            _motor.SetMoveDirection(Direction.Left);

        else if (Input.GetKeyDown(KeyCode.S))
            _motor.SetMoveDirection(Direction.Down);

        else if (Input.GetKeyDown(KeyCode.D))
            _motor.SetMoveDirection(Direction.Right);

    }
}
