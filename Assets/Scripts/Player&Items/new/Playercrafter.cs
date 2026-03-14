using System.Collections.Generic;
using UnityEngine;

public class PlayerCrafter : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInventory _inventory;

    [Header("Recipes")]
    [Tooltip("Drag all CraftingRecipeData ScriptableObjects here.")]
    [SerializeField] private List<CraftingRecipeData> _recipes;

    // ── Events ───────────────────────────────────────────────────────────────
    /// <summary>Fired after a successful craft. Passes the resulting ItemData.</summary>
    public System.Action<ItemData> OnItemCrafted;

    // ── Internal lookup ──────────────────────────────────────────────────────
    private Dictionary<string, CraftingRecipeData> _recipeLookup;

    // ── Unity lifecycle ──────────────────────────────────────────────────────

    private void Awake()
    {
        BuildRecipeLookup();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            TryCraft();
    }

    // ── Private methods ──────────────────────────────────────────────────────

    private void BuildRecipeLookup()
    {
        _recipeLookup = new Dictionary<string, CraftingRecipeData>(_recipes?.Count ?? 0);

        if (_recipes == null) return;

        foreach (var recipe in _recipes)
        {
            if (recipe == null) continue;

            var key = recipe.GetKey();

            if (string.IsNullOrEmpty(key))
            {
                Debug.LogWarning($"[PlayerCrafter] Recipe '{recipe.name}' has missing ingredients — skipping.");
                continue;
            }

            if (!_recipeLookup.TryAdd(key, recipe))
            {
                Debug.LogWarning($"[PlayerCrafter] Duplicate recipe key '{key}' in '{recipe.name}' — skipping.");
            }
        }

        Debug.Log($"[PlayerCrafter] {_recipeLookup.Count} recipes loaded.");
    }

    private void TryCraft()
    {
        var left  = _inventory.LeftHandItemName;
        var right = _inventory.RightHandItemName;

        if (string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right))
        {
            Debug.Log("[PlayerCrafter] Crafting requires an item in each hand.");
            return;
        }

        // Build canonical key (sorted, order-independent)
        var key = string.Compare(left, right, System.StringComparison.Ordinal) <= 0
            ? $"{left}+{right}"
            : $"{right}+{left}";

        if (!_recipeLookup.TryGetValue(key, out var recipe))
        {
            Debug.Log($"[PlayerCrafter] No recipe found for: {key}");
            return;
        }

        // Consume ingredients
        _inventory.ClearBothHands();

        // Produce result
        var spawned = _inventory.TrySpawnIntoHand(recipe.result);

        if (spawned)
        {
            Debug.Log($"[PlayerCrafter] Crafted: {recipe.result.itemName}");
            OnItemCrafted?.Invoke(recipe.result);
        }
        else
        {
            Debug.LogWarning("[PlayerCrafter] Crafting succeeded but could not spawn result — hands full?");
        }
    }
}