using System;
using UnityEngine;

public class EatGhost : MonoBehaviour
{
    public event Action<int> OnEatGhost;

    public int EatGhostValue;

    private GhostAI[] _ghostAI;
    private int _totalGhostsEated = 0;
    private void Start()
    {
        _totalGhostsEated = 0;

        _ghostAI = FindObjectsOfType<GhostAI>();
        foreach (var ghost in _ghostAI)
        {
            ghost.OnGhostStateChanged += GhostAI_OnGhostStateDefeated;
        }

    }

    private void GhostAI_OnGhostStateDefeated(GhostState state)
    {
        if (state == GhostState.Defeated)
        {
            _totalGhostsEated++;
            var totalScore = (EatGhostValue * _totalGhostsEated);
            OnEatGhost?.Invoke(totalScore);

            Debug.Log($"Fantasmas devorado:{_totalGhostsEated} | +{totalScore}");
        }
    }
}
