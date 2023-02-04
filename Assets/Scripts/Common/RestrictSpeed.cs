using UnityEngine;

public class RestrictSpeed : MonoBehaviour
{
    public float RestrictVelocity;
    private float _normalVelocity;

    private CharacterMotor _motor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        _motor = other.GetComponent<CharacterMotor>();
        _normalVelocity = _motor.MoveSpeed;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        _motor.MoveSpeed = RestrictVelocity;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        _motor.MoveSpeed = _normalVelocity;
    }
}
