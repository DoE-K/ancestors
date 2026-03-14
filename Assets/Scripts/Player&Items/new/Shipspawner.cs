using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    [Header("Ship")]
    [SerializeField] private GameObject _shipObject;

    [Header("Settings")]
    [SerializeField] private int _scoreThreshold = 100;

    private bool _shipSpawned = false;

    // ── Unity lifecycle ──────────────────────────────────────────────────────

    private void OnEnable()
    {
        GlobalScore.OnScoreChanged += HandleScoreChanged;
    }

    private void OnDisable()
    {
        GlobalScore.OnScoreChanged -= HandleScoreChanged;
    }

    // ── Event handler ────────────────────────────────────────────────────────

    private void HandleScoreChanged(int newScore)
    {
        if (_shipSpawned) return;
        if (newScore < _scoreThreshold) return;

        _shipObject.SetActive(true);
        _shipSpawned = true;

        Debug.Log($"[ShipSpawner] Score {newScore} reached threshold — Ship activated.");
    }
}