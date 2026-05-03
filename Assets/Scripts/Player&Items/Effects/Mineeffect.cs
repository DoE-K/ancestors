using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Tool effect for pickaxe / hammerstone on cave walls.
/// </summary>
[CreateAssetMenu(fileName = "MineEffect", menuName = "Survival/Effects/Mine")]
public class MineEffect : ItemEffect
{
    [System.Serializable]
    public class OreEntry
    {
        public ItemData item;
        [Tooltip("Relative weight. Higher = more common. e.g. Stein=10, Gold=3, Diamant=1")]
        [Min(0f)]
        public float weight = 10f;
    }

    [Header("Mining Settings")]
    public List<OreEntry> possibleOres;

    [Range(0f, 1f)]
    [Tooltip("Chance that any ore drops at all per swing.")]
    public float dropChance = 0.6f;

    [Header("Spawn Settings")]
    [Tooltip("Ore spawns within this radius around the player.")]
    public float spawnRadius = 1.2f;

    //public override string actionHint => "Abbauen";

    public override void Use(PlayerContext ctx)
    {
        if (!ctx.IsNearWall)
        {
            Debug.Log("[MineEffect] Keine Hohlenwand in der Naehe.");
            return;
        }

        if (possibleOres == null || possibleOres.Count == 0)
        {
            Debug.LogWarning("[MineEffect] Keine Erze konfiguriert!");
            return;
        }

        if (Random.value > dropChance)
        {
            Debug.Log("[MineEffect] Swing — nichts gefunden.");
            return;
        }

        var oreData = GetWeightedRandom();
        if (oreData?.prefab == null) return;

        // Spawn at random position around player
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos     = ctx.Transform.position
            + new Vector3(randomOffset.x, randomOffset.y, 0f);

        Object.Instantiate(oreData.prefab, spawnPos, Quaternion.identity);

        Debug.Log($"[MineEffect] Abgebaut: {oreData.itemName}");
    }

    private ItemData GetWeightedRandom()
    {
        float total = 0f;
        foreach (var entry in possibleOres)
            if (entry?.item != null) total += entry.weight;

        if (total <= 0f) return null;

        float roll       = Random.Range(0f, total);
        float cumulative = 0f;

        foreach (var entry in possibleOres)
        {
            if (entry?.item == null) continue;
            cumulative += entry.weight;
            if (roll <= cumulative) return entry.item;
        }

        return possibleOres[possibleOres.Count - 1].item;
    }
}