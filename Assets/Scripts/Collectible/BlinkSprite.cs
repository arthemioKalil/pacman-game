using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlinkSprite : MonoBehaviour
{
    public float Interval;

    private SpriteRenderer _spriteRenderer;

    private float _nextStateChange;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = true;
        _nextStateChange = Time.time + Interval;
    }

    private void Update()
    {
        if (Time.time > _nextStateChange)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            _nextStateChange = Time.time + Interval;
        }
    }
}
