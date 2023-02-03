using UnityEngine;

public class PacmanView : MonoBehaviour
{
    public CharacterMotor CharacterMotor;

    public Animator Animator;

    private void Start()
    {
        CharacterMotor.OnDirectionChanged += CharacterMotor_OnDirectionChanged;
    }

    private void CharacterMotor_OnDirectionChanged(Direction direction)
    {

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
