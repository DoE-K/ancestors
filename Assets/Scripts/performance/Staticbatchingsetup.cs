using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class StaticBatchingSetup : EditorWindow
{
    [MenuItem("Tools/Setup Static Batching")]
    public static void ShowWindow() => GetWindow<StaticBatchingSetup>("Static Batching Setup");

    private string[] _staticTags = { "Grass", "StaticTree", "Decoration" };
    private string   _newTag     = "";

    private void OnGUI()
    {
        GUILayout.Label("Tags to mark as Static", EditorStyles.boldLabel);

        for (int i = 0; i < _staticTags.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            _staticTags[i] = EditorGUILayout.TextField(_staticTags[i]);
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                var list = new System.Collections.Generic.List<string>(_staticTags);
                list.RemoveAt(i);
                _staticTags = list.ToArray();
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        _newTag = EditorGUILayout.TextField("New Tag", _newTag);
        if (GUILayout.Button("Add", GUILayout.Width(60)) && !string.IsNullOrEmpty(_newTag))
        {
            var list = new System.Collections.Generic.List<string>(_staticTags) { _newTag };
            _staticTags = list.ToArray();
            _newTag = "";
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(12);
        EditorGUILayout.HelpBox(
            "This marks objects as BatchingStatic so Unity combines their draw calls.\n" +
            "Only use for objects that NEVER move at runtime.",
            MessageType.Info);

        if (GUILayout.Button("Apply Static Flags", GUILayout.Height(32)))
            ApplyStaticFlags();
    }

    private void ApplyStaticFlags()
    {
        int count = 0;

        foreach (var tag in _staticTags)
        {
            if (string.IsNullOrEmpty(tag)) continue;

            GameObject[] objects;
            try { objects = GameObject.FindGameObjectsWithTag(tag); }
            catch { Debug.LogWarning($"[StaticBatchingSetup] Tag '{tag}' does not exist."); continue; }

            foreach (var obj in objects)
            {
                GameObjectUtility.SetStaticEditorFlags(
                    obj, StaticEditorFlags.BatchingStatic);
                count++;
            }
        }

        Debug.Log($"[StaticBatchingSetup] Marked {count} objects as BatchingStatic.");
        AssetDatabase.SaveAssets();
    }
}
#endif

/// <summary>
/// Runtime component — call Initialise() once after scene load to apply batching.
/// Place on any persistent GameObject (e.g. GameManager).
/// </summary>
public class StaticBatchingRuntime : MonoBehaviour
{
    [Tooltip("Root GameObjects whose children should be combined. " +
             "Leave empty to batch the entire scene.")]
    [SerializeField] private GameObject[] _roots;

    private void Awake()
    {
        if (_roots == null || _roots.Length == 0)
            StaticBatchingUtility.Combine(gameObject.scene
                .GetRootGameObjects()[0]); // fallback: first root
        else
            foreach (var root in _roots)
                if (root != null)
                    StaticBatchingUtility.Combine(root);

        Debug.Log("[StaticBatchingRuntime] Static batching applied.");
    }
}