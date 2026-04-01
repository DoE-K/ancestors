using System.Collections.Generic;
using UnityEngine;

public class WorldChunkManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _player;

    [Header("Chunk Settings")]
    [Tooltip("Size of each chunk in Unity units. 50 = 50x50 area per chunk.")]
    [SerializeField] private float _chunkSize     = 50f;
    [Tooltip("How many chunks in each direction around the player to keep active.")]
    [SerializeField] private int   _activeRadius  = 2;

    [Header("Performance")]
    [Tooltip("Seconds between chunk activation checks. Lower = more responsive, higher = cheaper.")]
    [SerializeField] private float _checkInterval = 0.5f;

    [Header("Auto Registration")]
    [Tooltip("If true, all GameObjects tagged 'WorldObject' are auto-sorted into chunks on Start.")]
    [SerializeField] private bool  _autoRegister  = true;
    [SerializeField] private string _worldObjectTag = "WorldObject";

    // ── Internal state ────────────────────────────────────────────────────────
    private Dictionary<Vector2Int, List<GameObject>> _chunks
        = new Dictionary<Vector2Int, List<GameObject>>();

    private Vector2Int _lastPlayerChunk = new Vector2Int(int.MinValue, int.MinValue);

    // ── Unity lifecycle ───────────────────────────────────────────────────────

    private void Start()
    {
        if (_autoRegister)
            AutoRegisterWorldObjects();

        InvokeRepeating(nameof(CheckChunks), 0f, _checkInterval);
    }

    // ── Public API ────────────────────────────────────────────────────────────

    /// <summary>Manually register a GameObject into the correct chunk.</summary>
    public void Register(GameObject obj)
    {
        var key = GetChunkKey(obj.transform.position);
        if (!_chunks.ContainsKey(key))
            _chunks[key] = new List<GameObject>();

        _chunks[key].Add(obj);
    }

    // ── Private methods ───────────────────────────────────────────────────────

    private void AutoRegisterWorldObjects()
    {
        var all = GameObject.FindGameObjectsWithTag(_worldObjectTag);
        foreach (var obj in all)
            Register(obj);

        Debug.Log($"[WorldChunkManager] Registered {all.Length} objects across {_chunks.Count} chunks.");
    }

    private void CheckChunks()
    {
        if (_player == null) return;

        var playerChunk = GetChunkKey(_player.position);

        // Skip if player hasn't moved to a new chunk
        if (playerChunk == _lastPlayerChunk) return;
        _lastPlayerChunk = playerChunk;

        foreach (var kvp in _chunks)
        {
            bool shouldBeActive = IsInActiveRadius(kvp.Key, playerChunk);
            SetChunkActive(kvp.Value, shouldBeActive);
        }
    }

    private bool IsInActiveRadius(Vector2Int chunkKey, Vector2Int playerChunk)
    {
        return Mathf.Abs(chunkKey.x - playerChunk.x) <= _activeRadius
            && Mathf.Abs(chunkKey.y - playerChunk.y) <= _activeRadius;
    }

    private void SetChunkActive(List<GameObject> objects, bool active)
    {
        foreach (var obj in objects)
        {
            if (obj == null) continue;
            if (obj.activeSelf != active)
                obj.SetActive(active);
        }
    }

    private Vector2Int GetChunkKey(Vector2 position)
    {
        return new Vector2Int(
            Mathf.FloorToInt(position.x / _chunkSize),
            Mathf.FloorToInt(position.y / _chunkSize)
        );
    }

    private Vector2Int GetChunkKey(Vector3 position) =>
        GetChunkKey(new Vector2(position.x, position.y));

    // ── Editor visualisation ──────────────────────────────────────────────────

    private void OnDrawGizmosSelected()
    {
        if (_player == null) return;

        var playerChunk = GetChunkKey(_player.position);

        for (int x = -_activeRadius; x <= _activeRadius; x++)
        {
            for (int y = -_activeRadius; y <= _activeRadius; y++)
            {
                var chunkPos = new Vector3(
                    (playerChunk.x + x) * _chunkSize,
                    (playerChunk.y + y) * _chunkSize, 0f);

                Gizmos.color = new Color(0f, 1f, 0f, 0.08f);
                Gizmos.DrawCube(
                    chunkPos + new Vector3(_chunkSize / 2f, _chunkSize / 2f, 0f),
                    new Vector3(_chunkSize, _chunkSize, 0f));

                Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
                Gizmos.DrawWireCube(
                    chunkPos + new Vector3(_chunkSize / 2f, _chunkSize / 2f, 0f),
                    new Vector3(_chunkSize, _chunkSize, 0f));
            }
        }
    }
}