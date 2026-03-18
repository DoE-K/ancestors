using System.Collections.Generic;
using UnityEngine;

public class CaveGenerator : MonoBehaviour
{
    [Header("Segment Settings")]
    [SerializeField] private GameObject _segmentPrefab;
    [SerializeField] private float      _segmentLength          = 35f;
    [SerializeField] private int        _segmentsAhead          = 2;
    [SerializeField] private float      _startY                 = -175f;

    [Header("Player Tracking")]
    [SerializeField] private Transform  _player;
    [Tooltip("How many units ahead of the player (per segment) before a new segment spawns.")]
    [SerializeField] private float      _playerLookaheadPerSegment = 25f;

    [Header("Ore Spawning")]
    [SerializeField] private OreSpawnTable _oreSpawnTable;

    [Header("Cleanup")]
    [Tooltip("How many segments behind the player to keep before destroying old ones.")]
    [SerializeField] private int _segmentsToKeepBehind = 2;

    private float                _lastY;
    private Queue<GameObject>    _activeSegments = new Queue<GameObject>();

    // ── Unity lifecycle ──────────────────────────────────────────────────────

    private void Start()
    {
        _lastY = _startY;

        for (int i = 0; i < _segmentsAhead; i++)
            SpawnSegment();
    }

    private void Update()
    {
        if (_player.position.y - (_playerLookaheadPerSegment * _segmentsAhead) < _lastY)
            SpawnSegment();
    }

    // ── Private methods ──────────────────────────────────────────────────────

    private void SpawnSegment()
    {
        var seg = Instantiate(_segmentPrefab, new Vector3(0f, _lastY, 0f), Quaternion.identity);
        _lastY -= _segmentLength;

        SpawnOres(seg);

        _activeSegments.Enqueue(seg);

        CleanupOldSegments();
    }

    private void SpawnOres(GameObject segment)
    {
        if (_oreSpawnTable == null) return;

        var caveSegment = segment.GetComponent<CaveSegment>();
        if (caveSegment == null)
        {
            Debug.LogWarning("[CaveGenerator] Segment prefab is missing a CaveSegment component.");
            return;
        }

        foreach (var spawnPoint in caveSegment.orePoints)
        {
            var ore = _oreSpawnTable.GetRandomOre();
            if (ore?.prefab == null) continue; // empty slot — skip

            Instantiate(ore.prefab, spawnPoint.position, Quaternion.identity, segment.transform);
        }
    }

    private void CleanupOldSegments()
    {
        int maxSegments = _segmentsAhead + _segmentsToKeepBehind;

        while (_activeSegments.Count > maxSegments)
        {
            var old = _activeSegments.Dequeue();
            if (old != null) Destroy(old);
        }
    }
}