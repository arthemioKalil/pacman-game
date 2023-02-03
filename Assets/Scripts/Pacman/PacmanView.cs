using UnityEngine;

public class PacmanView : MonoBehaviour
{
    public CharacterMotor CharacterMotor;

    public Life CharacterLife;

    public Animator Animator;
    public AudioSource AudioSource;
    public AudioClip LifeLostSound;

    private void Start()
    {
        CharacterMotor.OnDirectionChanged += CharacterMotor_OnDirectionChanged;
        CharacterLife.OnLifeRemoved += CharacterLife_OnLifeRemoved;
        CharacterMotor.OnResetPosition += CharacterMotor_OnResetPosition;
        CharacterMotor.OnDisabled += CharacterMotor_OnDisabled;

        Animator.SetBool("Moving", false);
        Animator.SetBool("Dead", false);
    }

    private void CharacterMotor_OnDisabled()
    {
        Animator.speed = 0;
    }

    private void CharacterMotor_OnResetPosition()
    {
        Animator.SetBool("Moving", false);
        Animator.SetBool("Dead", false);
    }

    private void CharacterLife_OnLifeRemoved(int _)
    {
        transform.Rotate(0, 0, -90);
        AudioSource.PlayOneShot(LifeLostSound);
        Animator.speed = 1;
        Animator.SetBool("Moving", false);
        Animator.SetBool("Dead", true);
    }

    private void CharacterMotor_OnDirectionChanged(Direction direction)
    {
        Animator.speed = 1;

        switch (direction)
        {
            case Direction.Up:
                Animator.SetBool("Moving", true);
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.Left:
                Animator.SetBool("Moving", true);
                transform.rotation = Quaternion.Euler(0, 0, 180);

                break;
            case Direction.Down:
                Animator.SetBool("Moving", true);
                transform.rotation = Quaternion.Euler(0, 0, 270);

                break;
            case Direction.Right:
                Animator.SetBool("Moving", true);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

            default:
            case Direction.None:
                Animator.SetBool("Moving", false);
                break;

        }
    }
}
