using System;

public static class GlobalScore
{
    /// <summary>Fired every time the score changes. Passes the new score value.</summary>
    public static event Action<int> OnScoreChanged;

    private static int _score = 0;

    /// <summary>Current score. Read from anywhere, write only via AddScore().</summary>
    public static int Score => _score;

    /// <summary>
    /// Resets score to zero (call at scene start from Player.cs).
    /// Also fires OnScoreChanged so listeners reset cleanly.
    /// </summary>
    public static void Reset()
    {
        _score = 0;
        OnScoreChanged?.Invoke(_score);
    }

    /// <summary>Adds points and notifies all listeners.</summary>
    public static void AddScore(int points)
    {
        if (points <= 0) return;
        _score += points;
        OnScoreChanged?.Invoke(_score);
    }
}