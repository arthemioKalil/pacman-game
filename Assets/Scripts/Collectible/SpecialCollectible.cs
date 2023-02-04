using UnityEngine;

public class SpecialCollectible : MonoBehaviour
{
    public float Duration;
    public float HyperSpeed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var _motor = other.GetComponent<CharacterMotor>();
        _motor.SetMotorSpeed(HyperSpeed, Duration);
    }

}
