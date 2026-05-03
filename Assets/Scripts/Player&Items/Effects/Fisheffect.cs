using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Tool effect for fishing rod.
/// Caught fish spawns on the ground near the player (random radius).
/// Player can then pick it up normally.
/// </summary>
[CreateAssetMenu(fileName = "FishEffect", menuName = "Survival/Effects/Fish")]
public class FishEffect : ItemEffect
{
    [System.Serializable]
    public class FishEntry
    {
        public ItemData item;
        [Tooltip("Relative weight. Higher = more common.")]
        [Min(0f)]
        public float weight = 10f;
    }

    [Header("Fishing Settings")]
    public List<FishEntry> possibleFish;

    [Range(0f, 1f)]
    [Tooltip("Chance that any fish is caught at all.")]
    public float catchChance = 0.7f;

    [Header("Spawn Settings")]
    [Tooltip("Fish lands within this radius around the player.")]
    public float spawnRadius = 1.5f;

    //public override string actionHint => "Angeln";

    public override void Use(PlayerContext ctx)
    {
        if (!ctx.IsNearWater)
        {
            Debug.Log("[FishEffect] Kein Wasser in der Naehe.");
            return;
        }

        if (possibleFish == null || possibleFish.Count == 0)
        {
            Debug.LogWarning("[FishEffect] Keine Fische konfiguriert!");
            return;
        }

        if (Random.value > catchChance)
        {
            Debug.Log("[FishEffect] Kein Biss diesmal...");
            return;
        }

        var fishData = GetWeightedRandom();
        if (fishData?.prefab == null) return;

        // Spawn at random position around player
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = ctx.Transform.position
            + new Vector3(randomOffset.x, randomOffset.y, 0f);

        Object.Instantiate(fishData.prefab, spawnPos, Quaternion.identity);

        Debug.Log($"[FishEffect] Gefangen: {fishData.itemName}");
    }

    private ItemData GetWeightedRandom()
    {
        float total = 0f;
        foreach (var entry in possibleFish)
            if (entry?.item != null) total += entry.weight;

        if (total <= 0f) return null;

        float roll       = Random.Range(0f, total);
        float cumulative = 0f;

        foreach (var entry in possibleFish)
        {
            if (entry?.item == null) continue;
            cumulative += entry.weight;
            if (roll <= cumulative) return entry.item;
        }

        return possibleFish[possibleFish.Count - 1].item;
    }
}