using System.Collections.Generic;
using UnityEngine;

public class PlayerCave : MonoBehaviour
{
    [Header("Cave Portals")]
    [SerializeField] private List<CavePortal> _portals;

    [Header("Dependencies")]
    [SerializeField] private PlayerInventory _inventory;

    [Header("Feedback (optional)")]
    [Tooltip("Message shown when the player tries to enter without the required item.")]
    [SerializeField] private string _lockedMessage = "You need a key to enter.";

    private Dictionary<string, CavePortal> _portalLookup;

    // ── Unity lifecycle ───────────────────────────────────────────────────────

    private void Awake()
    {
        BuildLookup();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("cave")) return;

        var cave = other.GetComponent<Cave>();
        if (cave == null) return;

        Teleport(cave);
    }

    // ── Private methods ───────────────────────────────────────────────────────

    private void BuildLookup()
    {
        _portalLookup = new Dictionary<string, CavePortal>(_portals?.Count ?? 0);
        if (_portals == null) return;

        foreach (var portal in _portals)
        {
            if (portal == null) continue;
            if (!_portalLookup.TryAdd(portal.caveName, portal))
                Debug.LogWarning($"[PlayerCave] Duplicate cave name '{portal.caveName}'.");
        }
    }

    private void Teleport(Cave cave)
    {
        if (!_portalLookup.TryGetValue(cave.caveName, out var portal))
        {
            Debug.LogWarning($"[PlayerCave] No CavePortal defined for '{cave.caveName}'.");
            return;
        }

        // Exits never require a key — only entrances do
        if (!cave.isExit && !HasRequiredItem(portal))
        {
            Debug.Log($"[PlayerCave] Entry blocked — {_lockedMessage}");
            // TODO: show _lockedMessage in UI
            return;
        }

        var destination = cave.isExit ? portal.exitDestination : portal.entryDestination;
        transform.position = new Vector3(destination.x, destination.y, 0f);
    }

    /// <summary>
    /// Returns true if the portal has no item requirement,
    /// or if the player holds the required item in either hand.
    /// </summary>
    private bool HasRequiredItem(CavePortal portal)
    {
        if (portal.requiredItem == null) return true;
        if (_inventory == null)          return true; // no inventory → always open

        string required = portal.requiredItem.itemName;

        return _inventory.RightHandItemName == required
            || _inventory.LeftHandItemName  == required;
    }
}