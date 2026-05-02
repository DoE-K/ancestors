using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Tool effect for fishing rod.
/// Simple: player touches water trigger → Use button erscheint → Fisch in Hand.
/// Each fish entry has a configurable weight for rarity.
/// </summary>
[CreateAssetMenu(fileName = "FishEffect", menuName = "Survival/Effects/Fish")]
public class FishEffect : ItemEffect
{
    [System.Serializable]
    public class FishEntry
    {
        public ItemData item;
        [Tooltip("Relative weight. Higher = more common. e.g. Fisch=10, Seltener Fisch=2")]
        [Min(0f)]
        public float weight = 10f;
    }

    [Header("Fishing Settings")]
    public List<FishEntry> possibleFish;

    [Range(0f, 1f)]
    [Tooltip("Chance that any fish is caught at all. Rest = nothing.")]
    public float catchChance = 0.7f;

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

        var fish = GetWeightedRandom();
        if (fish == null) return;

        ctx.Inventory.TrySpawnIntoHand(fish);
        Debug.Log($"[FishEffect] Gefangen: {fish.itemName}");
    }

    private ItemData GetWeightedRandom()
    {
        float total = 0f;
        foreach (var entry in possibleFish)
            if (entry?.item != null) total += entry.weight;

        if (total <= 0f) return null;

        float roll = Random.Range(0f, total);
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