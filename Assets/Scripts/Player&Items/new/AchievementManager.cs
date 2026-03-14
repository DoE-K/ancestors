using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private ScorePopup _scorePopup;

    [Header("Location Achievements")]
    [Tooltip("Add a LocationAchievement ScriptableObject for each special place.")]
    [SerializeField] private List<LocationAchievement> _locationAchievements;

    // Tracks which item names and location tags have already been awarded
    private readonly HashSet<string> _discoveredItems    = new HashSet<string>();
    private readonly HashSet<string> _visitedLocations   = new HashSet<string>();

    // ── Unity lifecycle ──────────────────────────────────────────────────────

    private void OnEnable()
    {
        if (_inventory == null)
        {
            Debug.LogError("[AchievementManager] PlayerInventory reference is missing!");
            return;
        }

        _inventory.OnItemPickedUp += HandleItemPickedUp;
    }

    private void OnDisable()
    {
        if (_inventory != null)
            _inventory.OnItemPickedUp -= HandleItemPickedUp;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleLocationVisit(other);
    }

    // ── Event handlers ───────────────────────────────────────────────────────

    private void HandleItemPickedUp(ItemData itemData)
    {
        if (itemData == null) return;
        if (_discoveredItems.Contains(itemData.itemName)) return;

        _discoveredItems.Add(itemData.itemName);
        AwardScore(itemData.discoveryScore);
    }

    private void HandleLocationVisit(Collider2D other)
    {
        if (_locationAchievements == null) return;

        foreach (var achievement in _locationAchievements)
        {
            if (achievement == null) continue;
            if (!other.CompareTag(achievement.targetTag)) continue;
            if (_visitedLocations.Contains(achievement.targetTag)) continue;

            _visitedLocations.Add(achievement.targetTag);
            AwardScore(achievement.scoreReward);
            return;
        }
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private void AwardScore(int points)
    {
        if (points <= 0) return;

        GlobalScore.AddScore(points);
        _scorePopup?.Show(points);
    }

    // ── Debug / Editor helper ────────────────────────────────────────────────

    public bool IsItemDiscovered(string itemName) => _discoveredItems.Contains(itemName);

    /// <summary>How many unique items have been discovered so far.</summary>
    public int DiscoveredItemCount => _discoveredItems.Count;
}