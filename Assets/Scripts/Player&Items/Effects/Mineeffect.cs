using UnityEngine;

/// <summary>
/// Tool effect for pickaxe / hammerstone used on cave walls.
/// Casts a short raycast in the player's facing direction.
/// If it hits a tagged "CaveWall", spawns a random ore item nearby.
///
/// Create via: Right-click → Create → Survival/Effects/Mine
/// Assign to: Schaufel_ItemData, Hammerstein_ItemData etc.
/// </summary>
[CreateAssetMenu(fileName = "MineEffect", menuName = "Survival/Effects/Mine")]
public class MineEffect : ItemEffect
{
    [Header("Mining Settings")]
    [Tooltip("How far the player can reach to mine.")]
    public float          mineRange   = 1.5f;
    [Tooltip("Tag on cave wall objects.")]
    public string         wallTag     = "CaveWall";
    [Tooltip("Possible ore items to drop. One is picked randomly.")]
    public ItemData[]     possibleOres;
    [Tooltip("Chance (0–1) to get any ore per swing. Rest = nothing.")]
    [Range(0f, 1f)]
    public float          dropChance  = 0.6f;

    public override void Use(PlayerContext ctx)
    {
        // Raycast in all 4 directions — pick nearest hit
        Vector2[] dirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        RaycastHit2D bestHit = default;
        float        bestDist = float.MaxValue;

        foreach (var dir in dirs)
        {
            var hit = Physics2D.Raycast(ctx.Position, dir, mineRange);
            if (!hit || !hit.collider.CompareTag(wallTag)) continue;
            if (hit.distance < bestDist)
            {
                bestDist = hit.distance;
                bestHit  = hit;
            }
        }

        if (!bestHit) return;

        if (Random.value > dropChance)
        {
            Debug.Log("[MineEffect] Swing — nothing dropped this time.");
            return;
        }

        if (possibleOres == null || possibleOres.Length == 0) return;

        var ore     = possibleOres[Random.Range(0, possibleOres.Length)];
        var spawnPos = bestHit.point + bestHit.normal * -0.5f;

        if (ore.prefab != null)
            Instantiate(ore.prefab, spawnPos, Quaternion.identity);

        Debug.Log($"[MineEffect] Mined: {ore.itemName}");
    }
}