using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Survival/Crafting Recipe")]
public class CraftingRecipeData : ScriptableObject
{
    [Header("Ingredients (order does not matter)")]
    public ItemData ingredientA;
    public ItemData ingredientB;

    [Header("Result")]
    public ItemData result;

    public string GetKey()
    {
        if (ingredientA == null || ingredientB == null) return string.Empty;

        var a = ingredientA.itemName;
        var b = ingredientB.itemName;

        // Sort so "Stone+Branch" and "Branch+Stone" produce the same key
        return string.Compare(a, b, System.StringComparison.Ordinal) <= 0
            ? $"{a}+{b}"
            : $"{b}+{a}";
    }
}