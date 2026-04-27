using UnityEngine;

/// <summary>
/// ScriptableObject that defines all data for a single item type.
/// Create via: Right-click in Project → Create → Survival/Item Data
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Survival/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Identity")]
    public string itemName;

    [Header("UI")]
    public Sprite sprite;

    [Header("Prefab")]
    public GameObject prefab;

    [Header("Pickup Settings")]
    public bool destroyOnPickup = true;
    public bool ableToPickup    = true;

    [Header("Score")]
    public int discoveryScore = 10;

    [Header("Tool Effect (optional)")]
    [Tooltip("If set, this item can be activated as a tool. " +
             "Leave empty for regular items.")]
    public ItemEffect effect;

    public bool IsTool => effect != null;
}