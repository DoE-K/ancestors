using System.Collections.Generic;
using UnityEngine;

public class AnimalMemory : MonoBehaviour
{
    [System.Serializable]
    public class MemoryEntry
    {
        public Vector2 position;
        public float   timeRemaining;

        public MemoryEntry(Vector2 pos, float ttl)
        {
            position      = pos;
            timeRemaining = ttl;
        }
    }

    [Header("Settings")]
    [Tooltip("How long (seconds) a memory stays valid before fading.")]
    [SerializeField] private float _memoryDuration = 120f;
    [Tooltip("How close the animal must be to a memory position to 'arrive' and verify it.")]
    [SerializeField] private float _arrivalThreshold = 1.5f;
    [Tooltip("Max number of food memories kept (oldest dropped first).")]
    [SerializeField] private int   _maxFoodMemories  = 5;
    [Tooltip("Max number of water memories kept.")]
    [SerializeField] private int   _maxWaterMemories = 5;

    // ── Public read-only ─────────────────────────────────────────────────────
    public bool HasFoodMemory  => _foodMemories.Count  > 0;
    public bool HasWaterMemory => _waterMemories.Count > 0;

    // ── Private state ────────────────────────────────────────────────────────
    private readonly List<MemoryEntry> _foodMemories  = new List<MemoryEntry>();
    private readonly List<MemoryEntry> _waterMemories = new List<MemoryEntry>();

    // ── Unity lifecycle ──────────────────────────────────────────────────────

    private void Update()
    {
        DecayMemories(_foodMemories);
        DecayMemories(_waterMemories);
    }

    // ── Public API ───────────────────────────────────────────────────────────

    /// <summary>Store a food location in memory.</summary>
    public void RememberFood(Vector2 position)
    {
        Remember(_foodMemories, position, _maxFoodMemories);
    }

    /// <summary>Store a water location in memory.</summary>
    public void RememberWater(Vector2 position)
    {
        Remember(_waterMemories, position, _maxWaterMemories);
    }

    /// <summary>
    /// Returns the closest remembered food position, or null if none known.
    /// </summary>
    public Vector2? GetBestFoodMemory()
    {
        return GetNearest(_foodMemories);
    }

    /// <summary>
    /// Returns the closest remembered water position, or null if none known.
    /// </summary>
    public Vector2? GetBestWaterMemory()
    {
        return GetNearest(_waterMemories);
    }

    /// <summary>
    /// Call this when the animal arrives at a remembered food position
    /// but finds nothing there — removes the stale entry.
    /// </summary>
    public void ForgetFoodAt(Vector2 position)
    {
        ForgetAt(_foodMemories, position);
    }

    /// <summary>
    /// Call this when the animal arrives at a remembered water position
    /// but finds nothing there.
    /// </summary>
    public void ForgetWaterAt(Vector2 position)
    {
        ForgetAt(_waterMemories, position);
    }

    // ── Private helpers ──────────────────────────────────────────────────────

    private void Remember(List<MemoryEntry> memories, Vector2 position, int maxCount)
    {
        // Don't add duplicates that are very close to an existing memory
        foreach (var m in memories)
        {
            if (Vector2.Distance(m.position, position) < _arrivalThreshold)
            {
                // Refresh TTL instead of adding a duplicate
                m.timeRemaining = _memoryDuration;
                return;
            }
        }

        // Drop oldest memory if at capacity
        if (memories.Count >= maxCount)
            memories.RemoveAt(0);

        memories.Add(new MemoryEntry(position, _memoryDuration));
    }

    private Vector2? GetNearest(List<MemoryEntry> memories)
    {
        if (memories.Count == 0) return null;

        Vector2? best     = null;
        float    bestDist = float.MaxValue;

        foreach (var m in memories)
        {
            float dist = Vector2.Distance(transform.position, m.position);
            if (dist < bestDist)
            {
                bestDist = dist;
                best     = m.position;
            }
        }

        return best;
    }

    private void ForgetAt(List<MemoryEntry> memories, Vector2 position)
    {
        memories.RemoveAll(m =>
            Vector2.Distance(m.position, position) < _arrivalThreshold);
    }

    private void DecayMemories(List<MemoryEntry> memories)
    {
        for (int i = memories.Count - 1; i >= 0; i--)
        {
            memories[i].timeRemaining -= Time.deltaTime;
            if (memories[i].timeRemaining <= 0f)
                memories.RemoveAt(i);
        }
    }

    // ── Debug ────────────────────────────────────────────────────────────────

    public List<MemoryEntry> FoodMemories  => _foodMemories;
    public List<MemoryEntry> WaterMemories => _waterMemories;
}