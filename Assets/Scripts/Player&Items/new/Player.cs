using UnityEngine;

public class Player : MonoBehaviour
{
    private void Start()
    {
        GlobalScore.Reset();
    }

    /// <summary>
    /// Persists the final score and player name to PlayerPrefs so the
    /// ScoreManager in the highscore scene can pick it up on load.
    /// Called from Ship.cs when the scene transition begins.
    /// </summary>
    public void SubmitHighscore()
    {
        string playerName = PlayerPrefs.GetString("playerName", "XXX");

        PlayerPrefs.SetString("lastPlayerName", playerName);
        PlayerPrefs.SetInt("lastPlayerScore", GlobalScore.Score);
        PlayerPrefs.Save();

        // ScoreManager in the next scene reads GlobalScore.Score > 0
        // in its Start() and calls SaveFinalScore() automatically
    }
}