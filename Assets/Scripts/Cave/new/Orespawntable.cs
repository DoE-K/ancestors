using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ore Spawn Table", menuName = "Survival/Ore Spawn Table")]
public class OreSpawnTable : ScriptableObject
{
    [Tooltip("All possible ores and their relative weights.")]
    public List<OreData> ores = new List<OreData>();

    [Tooltip("Chance (0–1) that a spawn point produces nothing at all.")]
    [Range(0f, 1f)]
    public float emptyChance = 0.4f;

    private float _totalWeight = -1f;

    private void OnEnable() => _totalWeight = -1f;

    public OreData GetRandomOre()
    {
        if (Random.value < emptyChance) return null;
        if (ores == null || ores.Count == 0) return null;

        float total = GetTotalWeight();
        if (total <= 0f) return null;

        float roll = Random.Range(0f, total);
        float cumulative = 0f;

        foreach (var ore in ores)
        {
            if (ore == null) continue;
            cumulative += ore.weight;
            if (roll <= cumulative) return ore;
        }

        return ores[ores.Count - 1]; // fallback
    }

    private float GetTotalWeight()
    {
        if (_totalWeight >= 0f) return _totalWeight;

        _totalWeight = 0f;
        foreach (var ore in ores)
        {
            if (ore != null) _totalWeight += ore.weight;
        }
        return _totalWeight;
    }
}