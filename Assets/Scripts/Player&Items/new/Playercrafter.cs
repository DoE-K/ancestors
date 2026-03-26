using System.Collections.Generic;
using UnityEngine;

public class PlayerCrafter : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInventory _inventory;

    [Header("Recipes")]
    [SerializeField] private List<CraftingRecipeData> _recipes;

    public System.Action<ItemData> OnItemCrafted;

    private Dictionary<string, CraftingRecipeData> _recipeLookup;

    private void Awake()
    {
        BuildRecipeLookup();
    }

    // ── Public API — called by PlayerActionUI ─────────────────────────────────

    /// <summary>
    /// Returns true if the two held items form a valid recipe.
    /// Used by PlayerActionUI to decide whether to show the Craft button.
    /// </summary>
    public bool CanCraft()
    {
        var left  = _inventory.LeftHandItemName;
        var right = _inventory.RightHandItemName;

        if (string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right)) return false;

        return _recipeLookup.ContainsKey(MakeKey(left, right));
    }

    /// <summary>
    /// Attempts to craft using the two held items.
    /// Called by the Craft UI button.
    /// </summary>
    public void TryCraft()
    {
        var left  = _inventory.LeftHandItemName;
        var right = _inventory.RightHandItemName;

        if (string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right)) return;

        var key = MakeKey(left, right);
        if (!_recipeLookup.TryGetValue(key, out var recipe))
        {
            Debug.Log($"[PlayerCrafter] No recipe for: {key}");
            return;
        }

        _inventory.ClearBothHands();

        if (_inventory.TrySpawnIntoHand(recipe.result))
        {
            Debug.Log($"[PlayerCrafter] Crafted: {recipe.result.itemName}");
            OnItemCrafted?.Invoke(recipe.result);
        }
    }

    // ── Private ───────────────────────────────────────────────────────────────

    private void BuildRecipeLookup()
    {
        _recipeLookup = new Dictionary<string, CraftingRecipeData>(_recipes?.Count ?? 0);

        if (_recipes == null) return;

        foreach (var recipe in _recipes)
        {
            if (recipe == null) continue;
            var key = recipe.GetKey();
            if (string.IsNullOrEmpty(key)) continue;

            if (!_recipeLookup.TryAdd(key, recipe))
                Debug.LogWarning($"[PlayerCrafter] Duplicate recipe: '{key}'");
        }
    }

    private string MakeKey(string a, string b) =>
        string.Compare(a, b, System.StringComparison.Ordinal) <= 0
            ? $"{a}+{b}"
            : $"{b}+{a}";
}