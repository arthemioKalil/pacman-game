using UnityEngine;

public class SpecialSpeed : MonoBehaviour
{
    private float _timer;
    private bool _speedUp = false;
    private float _speedUpDuration;

    private CharacterMotor _motor;

    void Start()
    {
        _motor = GetComponent<CharacterMotor>();
        _motor.OnSpeedSpecial += Motor_OnSpeedSpecial;
    }

    private void Motor_OnSpeedSpecial(float duration)
    {
        if (!_speedUp)
        {
            _speedUpDuration = duration;
            _speedUp = true;
        }
    }

    void Update()
    {
        if (_speedUp)
        {
            _timer += Time.deltaTime;

            if (_timer >= _speedUpDuration)
            {
                _motor.MoveSpeed = _motor.NaturalSpeed;
                _timer = 0;
                _speedUp = false;
            }
        }
    }
}
