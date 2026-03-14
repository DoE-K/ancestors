using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScoreManager _scoreManager;

    private void Start()
    {
        GlobalScore.Reset();
    }

    /// <summary>
    /// Call this when the round ends to submit the final score.
    /// Can be called from Ship.cs or a future GameManager.
    /// </summary>
    public void SubmitHighscore()
    {
        _scoreManager?.SaveFinalScore(PlayerPrefs.GetString("playerName", "XXX"));
    }
}