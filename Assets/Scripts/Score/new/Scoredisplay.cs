using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    private void OnEnable()
    {
        GlobalScore.OnScoreChanged += UpdateDisplay;
        UpdateDisplay(GlobalScore.Score); // sync immediately on enable
    }

    private void OnDisable()
    {
        GlobalScore.OnScoreChanged -= UpdateDisplay;
    }

    private void UpdateDisplay(int newScore)
    {
        _scoreText.text = "Score: " + newScore;
    }
}