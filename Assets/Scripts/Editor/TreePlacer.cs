using UnityEngine;
using UnityEditor;

public class TreePlacer : EditorWindow
{
    public GameObject treePrefab;
    public int treeCount = 100;
    public Vector2 spawnAreaMin = new Vector2(-50, -50);
    public Vector2 spawnAreaMax = new Vector2(50, 50);

    [MenuItem("Tools/Random Trees Placer")]
    public static void ShowWindow()
    {
        GetWindow<TreePlacer>("Tree Placer");
    }

    void OnGUI()
    {
        treePrefab = (GameObject)EditorGUILayout.ObjectField("Tree Prefab", treePrefab, typeof(GameObject), false);
        treeCount = EditorGUILayout.IntField("Tree Count", treeCount);
        spawnAreaMin = EditorGUILayout.Vector2Field("Min Pos (X/Z)", spawnAreaMin);
        spawnAreaMax = EditorGUILayout.Vector2Field("Max Pos (X/Z)", spawnAreaMax);

        if (GUILayout.Button("Place Trees"))
        {
            PlaceTrees();
        }
    }

    void PlaceTrees()
    {
        if (treePrefab == null)
        {
            Debug.LogError("Bitte ein Tree Prefab zuweisen!");
            return;
        }

        for (int i = 0; i < treeCount; i++)
        {
            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector3 position = new Vector3(x, y, 0);

            GameObject newTree = (GameObject)PrefabUtility.InstantiatePrefab(treePrefab);
            newTree.transform.position = position;
            Undo.RegisterCreatedObjectUndo(newTree, "Tree Placement");
        }

        Debug.Log($"{treeCount} Bäume wurden platziert!");
    }
}
