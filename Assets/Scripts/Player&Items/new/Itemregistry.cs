using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemRegistry", menuName = "Survival/Item Registry")]
public class ItemRegistry : ScriptableObject
{
    [SerializeField] private List<ItemData> _items = new List<ItemData>();

    // Built at runtime for O(1) lookup
    private Dictionary<string, ItemData> _lookup;

    private void OnEnable()
    {
        BuildLookup();
    }

    private void BuildLookup()
    {
        _lookup = new Dictionary<string, ItemData>(_items.Count);
        foreach (var item in _items)
        {
            if (item == null) continue;

            if (!_lookup.TryAdd(item.itemName, item))
            {
                Debug.LogWarning($"[ItemRegistry] Duplicate item name detected: '{item.itemName}'. Skipping.");
            }
        }
    }

    /// <summary>Returns the ItemData for the given name, or null if not found.</summary>
    public ItemData Get(string itemName)
    {
        if (_lookup == null) BuildLookup();

        _lookup.TryGetValue(itemName, out var data);
        return data;
    }

    /// <summary>Returns true and the ItemData if found.</summary>
    public bool TryGet(string itemName, out ItemData data)
    {
        if (_lookup == null) BuildLookup();

        return _lookup.TryGetValue(itemName, out data);
    }

    public IEnumerable<ItemData> AllItems => _items;
}