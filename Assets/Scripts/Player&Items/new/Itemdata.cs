using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Survival/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Identity")]
    public string itemName;

    [Header("UI")]
    [Tooltip("Sprite shown in the hand button when this item is held.")]
    public Sprite sprite;

    [Header("Prefab")]
    public GameObject prefab;

    [Header("Pickup Settings")]
    public bool destroyOnPickup = true;
    public bool ableToPickup    = true;

    [Header("Score")]
    public int discoveryScore = 10;
}