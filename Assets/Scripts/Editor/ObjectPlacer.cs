using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ObjectPlacer : EditorWindow
{
    [MenuItem("Tools/Random Object Placer")]
    public static void ShowWindow() => GetWindow<ObjectPlacer>("Object Placer");

    private GameObject _prefab;
    private int        _count          = 100;
    private Vector2    _areaMin        = new Vector2(-50f, -50f);
    private Vector2    _areaMax        = new Vector2( 50f,  50f);

    private float      _pixelSize      = 0.09375f;
    private float      _gridOffset     = 0.375f;

    private bool       _preventOverlap = true;
    private float      _minDistance    = 1f;

    private bool       _useParent      = true;
    private string     _parentName     = "--- Placed Objects ---";
    private GameObject _parentObject;

    private void OnGUI()
    {
        GUILayout.Label("Prefab", EditorStyles.boldLabel);
        _prefab = (GameObject)EditorGUILayout.ObjectField(
            "Prefab", _prefab, typeof(GameObject), false);
        _count = EditorGUILayout.IntField("Count", _count);

        EditorGUILayout.Space(8);
        GUILayout.Label("Spawn Area", EditorStyles.boldLabel);
        _areaMin = EditorGUILayout.Vector2Field("Min (X / Y)", _areaMin);
        _areaMax = EditorGUILayout.Vector2Field("Max (X / Y)", _areaMax);

        EditorGUILayout.Space(8);
        GUILayout.Label("Pixel Grid", EditorStyles.boldLabel);
        _pixelSize  = EditorGUILayout.FloatField("Pixel Size (Unity units)", _pixelSize);
        _gridOffset = EditorGUILayout.FloatField("Grid Offset", _gridOffset);
        EditorGUILayout.HelpBox(
            $"1 pixel = {_pixelSize} units\n" +
            $"Grid offset = {_gridOffset}\n" +
            $"First valid position from 0: X = {_gridOffset}",
            MessageType.None);

        EditorGUILayout.Space(8);
        GUILayout.Label("Overlap Prevention", EditorStyles.boldLabel);
        _preventOverlap = EditorGUILayout.Toggle("Prevent Overlap", _preventOverlap);
        if (_preventOverlap)
            _minDistance = EditorGUILayout.FloatField("Min Distance", _minDistance);

        EditorGUILayout.Space(8);
        GUILayout.Label("Hierarchy", EditorStyles.boldLabel);
        _useParent  = EditorGUILayout.Toggle("Group Under Parent", _useParent);
        if (_useParent)
            _parentName = EditorGUILayout.TextField("Parent Name", _parentName);

        EditorGUILayout.Space(12);

        bool valid = _prefab != null && _areaMin.x < _areaMax.x && _areaMin.y < _areaMax.y;
        GUI.enabled = valid;
        if (GUILayout.Button("Place Objects", GUILayout.Height(32)))
            PlaceObjects();
        GUI.enabled = true;

        GUI.enabled = _useParent;
        if (GUILayout.Button("Clear Placed Objects"))
            ClearPlacedObjects();
        GUI.enabled = true;

        if (_prefab == null)
            EditorGUILayout.HelpBox("Assign a prefab.", MessageType.Warning);
        if (_areaMin.x >= _areaMax.x || _areaMin.y >= _areaMax.y)
            EditorGUILayout.HelpBox("Min must be smaller than Max.", MessageType.Error);
    }

    private void PlaceObjects()
    {
        if (_prefab == null) return;

        Transform parent      = _useParent ? GetOrCreateParent() : null;
        var       placed      = new List<Vector2>(_count);
        int       placedCount = 0;
        int       maxAttempts = _count * 10;
        int       attempts    = 0;

        Undo.SetCurrentGroupName("Place Objects");
        int undoGroup = Undo.GetCurrentGroup();

        while (placedCount < _count && attempts < maxAttempts)
        {
            attempts++;

            float rawX = Random.Range(_areaMin.x, _areaMax.x);
            float rawY = Random.Range(_areaMin.y, _areaMax.y);
            Vector2 pos = SnapToPixelGrid(new Vector2(rawX, rawY));

            if (_preventOverlap && IsTooClose(pos, placed))
                continue;

            var go = (GameObject)PrefabUtility.InstantiatePrefab(_prefab);
            go.transform.position = new Vector3(pos.x, pos.y, 0f);

            if (parent != null)
                go.transform.SetParent(parent, worldPositionStays: true);

            Undo.RegisterCreatedObjectUndo(go, "Place Object");
            placed.Add(pos);
            placedCount++;
        }

        Undo.CollapseUndoOperations(undoGroup);

        if (placedCount < _count)
            Debug.LogWarning($"[ObjectPlacer] Only placed {placedCount}/{_count}. " +
                             "Try a larger area or smaller Min Distance.");
        else
            Debug.Log($"[ObjectPlacer] Placed {placedCount} objects.");
    }

    private Vector2 SnapToPixelGrid(Vector2 pos)
    {
        return new Vector2(SnapAxis(pos.x), SnapAxis(pos.y));
    }

    private float SnapAxis(float value)
    {
        float shifted = value - _gridOffset;
        float snapped = Mathf.Round(shifted / _pixelSize) * _pixelSize;
        return snapped + _gridOffset;
    }

    private bool IsTooClose(Vector2 candidate, List<Vector2> existing)
    {
        foreach (var pos in existing)
            if (Vector2.Distance(candidate, pos) < _minDistance)
                return true;
        return false;
    }

    private Transform GetOrCreateParent()
    {
        if (_parentObject == null)
            _parentObject = GameObject.Find(_parentName);

        if (_parentObject == null)
        {
            _parentObject = new GameObject(_parentName);
            Undo.RegisterCreatedObjectUndo(_parentObject, "Create Parent");
        }

        return _parentObject.transform;
    }

    private void ClearPlacedObjects()
    {
        if (_parentObject == null)
            _parentObject = GameObject.Find(_parentName);

        if (_parentObject == null)
        {
            Debug.Log("[ObjectPlacer] No parent object found.");
            return;
        }

        Undo.DestroyObjectImmediate(_parentObject);
        _parentObject = null;
        Debug.Log("[ObjectPlacer] Cleared all placed objects.");
    }
}