using UnityEngine;
using UnityEditor;

public class ObjectPlacer : EditorWindow
{
    public GameObject ObjectPrefab;
    public int ObjectCount = 100;
    public Vector2 spawnAreaMin = new Vector2(-50, -50);
    public Vector2 spawnAreaMax = new Vector2(50, 50);

    [MenuItem("Tools/Random Object Placer")]
    public static void ShowWindow()
    {
        GetWindow<ObjectPlacer>("Object Placer");
    }

    void OnGUI()
    {
        ObjectPrefab = (GameObject)EditorGUILayout.ObjectField("Object Prefab", ObjectPrefab, typeof(GameObject), false);
        ObjectCount = EditorGUILayout.IntField("Object Count", ObjectCount);
        spawnAreaMin = EditorGUILayout.Vector2Field("Min Pos (X/Z)", spawnAreaMin);
        spawnAreaMax = EditorGUILayout.Vector2Field("Max Pos (X/Z)", spawnAreaMax);

        if (GUILayout.Button("Place Object"))
        {
            PlaceObject();
        }
    }

    void PlaceObject()
    {
        if (ObjectPrefab == null)
        {
            return;
        }

        for (int i = 0; i < ObjectCount; i++)
        {
            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector3 position = new Vector3(x, y, 0);

            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(ObjectPrefab);
            newObject.transform.position = position;
            Undo.RegisterCreatedObjectUndo(newObject, "Object Placement");
        }
    }
}
