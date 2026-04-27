using UnityEngine;

/// <summary>
/// Tool effect for fishing rod / net near water.
/// Player must be standing near a "Water" tagged object.
/// After a short wait, spawns a fish item in the player's hand.
///
/// Create via: Right-click → Create → Survival/Effects/Fish
/// Assign to: Angel_ItemData, Netz_ItemData
/// </summary>
[CreateAssetMenu(fileName = "FishEffect", menuName = "Survival/Effects/Fish")]
public class FishEffect : ItemEffect
{
    [Header("Fishing Settings")]
    public float      waterCheckRadius = 2f;
    public string     waterTag         = "Water";
    public ItemData[] possibleFish;
    [Range(0f, 1f)]
    public float      catchChance      = 0.7f;

    //public override string actionHint => "Angel auswerfen";

    public override void Use(PlayerContext ctx)
    {
        // Check if near water
        var cols = Physics2D.OverlapCircleAll(ctx.Position, waterCheckRadius);
        bool nearWater = false;
        foreach (var col in cols)
        {
            if (col.CompareTag(waterTag)) { nearWater = true; break; }
        }

        if (!nearWater)
        {
            Debug.Log("[FishEffect] Kein Wasser in der Nähe.");
            return;
        }

        if (Random.value > catchChance)
        {
            Debug.Log("[FishEffect] Kein Biss diesmal...");
            return;
        }

        if (possibleFish == null || possibleFish.Length == 0) return;

        var fish = possibleFish[Random.Range(0, possibleFish.Length)];
        ctx.Inventory.TrySpawnIntoHand(fish);

        Debug.Log($"[FishEffect] Gefangen: {fish.itemName}");
    }
}