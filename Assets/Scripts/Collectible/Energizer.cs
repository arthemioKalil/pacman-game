using UnityEngine;

public class Energizer : MonoBehaviour
{
    public float Duration;

    private void OnTriggerEnter2D(Collider2D _)
    {
        var ghosts = FindObjectsOfType<GhostAI>();

        foreach (var ghost in ghosts)
        {
            ghost.SetVulnerable(Duration);
        }
    }
}
