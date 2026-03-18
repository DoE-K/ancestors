using System.Collections.Generic;
using UnityEngine;

public class PlayerCave : MonoBehaviour
{
    [Header("Cave Portals")]
    [Tooltip("Add one CavePortal ScriptableObject per cave.")]
    [SerializeField] private List<CavePortal> _portals;

    private Dictionary<string, CavePortal> _portalLookup;

    // ── Unity lifecycle ──────────────────────────────────────────────────────

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

    // ── Private methods ──────────────────────────────────────────────────────

    private void BuildLookup()
    {
        _portalLookup = new Dictionary<string, CavePortal>(_portals?.Count ?? 0);

        if (_portals == null) return;

        foreach (var portal in _portals)
        {
            if (portal == null) continue;

            if (!_portalLookup.TryAdd(portal.caveName, portal))
            {
                Debug.LogWarning($"[PlayerCave] Duplicate cave name '{portal.caveName}' — skipping.");
            }
        }
    }

    private void Teleport(Cave cave)
    {
        if (!_portalLookup.TryGetValue(cave.caveName, out var portal))
        {
            Debug.LogWarning($"[PlayerCave] No CavePortal defined for '{cave.caveName}'.");
            return;
        }

        var destination = cave.isExit ? portal.exitDestination : portal.entryDestination;
        transform.position = new Vector3(destination.x, destination.y, 0f);
    }
}