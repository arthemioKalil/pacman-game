using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI CurrentScoreText;

    public TextMeshProUGUI HighScoreText;

    private void Start()
    {
        var scoreManager = FindObjectOfType<ScoreManager>();

        scoreManager.OnScoreChanged += ScoreManager_OnScoreChanged;
        scoreManager.OnHighScoreChanged += ScoreManager_OnHighScoreChanged;

        ScoreManager_OnHighScoreChanged(scoreManager.HightScore);
    }

    private void ScoreManager_OnHighScoreChanged(int HighScore)
    {
        HighScoreText.text = $"{HighScore:00}";
    }

    private void ScoreManager_OnScoreChanged(int currentScore)
    {
        CurrentScoreText.text = $"{currentScore:00}";
    }
}
