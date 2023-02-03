using UnityEngine;

[RequireComponent(typeof(GhostMove))]
public class GhostAI : MonoBehaviour
{
    private GhostMove _ghostMove;

    private Transform _pacman;

    public void StartMoving()
    {
        _ghostMove.CharacterMotor.enabled = true;
    }
    public void StopMoving()
    {
        _ghostMove.CharacterMotor.enabled = false;
    }
    public void Reset()
    {
        _ghostMove.CharacterMotor.ResetPosition();
    }

    void Awake()
    {
        _ghostMove = GetComponent<GhostMove>();
        _pacman = GameObject.FindWithTag("Player").transform;

        _ghostMove.OnUpdateMoveTarget += _ghostMove_OnUpdateMoveTarget;
    }

    private void _ghostMove_OnUpdateMoveTarget()
    {
        _ghostMove.SetTargetMoveLocation(_pacman.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Life>().RemoveLife();
        }
    }
}
