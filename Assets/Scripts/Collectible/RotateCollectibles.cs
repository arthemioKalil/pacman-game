using UnityEngine;

public class RotateCollectibles : MonoBehaviour
{
    private Transform _collectibleTransform;
    private float y = 150;
    private float _timerToRotate = 0.01f;
    private float _timer;
    void Start()
    {
        _collectibleTransform = this.transform;
    }

    void FixedUpdate()
    {
        _timer += Time.deltaTime;
        if (_timer >= _timerToRotate)
        {

            _collectibleTransform.Rotate(0, y * Time.fixedDeltaTime, 0);
            _timer = 0;
        }
    }
}
