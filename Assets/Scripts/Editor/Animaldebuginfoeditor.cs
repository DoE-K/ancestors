using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimalDebugInfo))]
public class AnimalDebugInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var info = (AnimalDebugInfo)target;

        if (Application.isPlaying)
            EditorUtility.SetDirty(target);

        // ── State ─────────────────────────────────────────────────────────────
        EditorGUILayout.Space(4);
        GUILayout.Label("Gedanken", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        DrawStateRow("Aktueller State",  info.CurrentState,  StateColor(info.CurrentState));
        DrawStateRow("Vorheriger State", info.PreviousState, Color.gray);
        if (info.ReproduceCooldown > 0f)
            EditorGUILayout.LabelField("Fortpflanzungs-Cooldown", $"{info.ReproduceCooldown:F1}s");
        EditorGUILayout.EndVertical();

        // ── Needs ─────────────────────────────────────────────────────────────
        EditorGUILayout.Space(6);
        GUILayout.Label("Bedürfnisse", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        DrawNeedBar("Hunger",  info.Hunger, info.IsHungry);
        DrawNeedBar("Durst",   info.Thirst, info.IsThirsty);
        DrawNeedBar("Energie", info.Energy, info.IsTired);
        float agePercent = info.MaxAge > 0f ? info.Age / info.MaxAge : 0f;
        EditorGUILayout.LabelField("Alter",
            $"{info.Age:F0}s / {info.MaxAge:F0}s  ({agePercent * 100f:F0}%)");
        EditorGUILayout.EndVertical();

        // ── Sensor ───────────────────────────────────────────────────────────
        EditorGUILayout.Space(6);
        GUILayout.Label("Wahrnehmung", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        DrawBoolRow("Futter sichtbar",  info.CanSeeFood);
        DrawBoolRow("Wasser sichtbar",  info.CanSeeWater);
        DrawBoolRow("Feind sichtbar",   info.CanSeePredator);
        DrawBoolRow("Partner sichtbar", info.CanSeeMate);
        DrawBoolRow("Nacht",            info.IsNight);
        EditorGUILayout.EndVertical();

        // ── Memory ───────────────────────────────────────────────────────────
        EditorGUILayout.Space(6);
        GUILayout.Label("Gedächtnis", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        DrawBoolRow($"Futter-Erinnerungen  ({info.FoodMemoryCount})",  info.HasFoodMemory);
        DrawBoolRow($"Wasser-Erinnerungen  ({info.WaterMemoryCount})", info.HasWaterMemory);
        EditorGUILayout.EndVertical();

        // ── Flags ────────────────────────────────────────────────────────────
        EditorGUILayout.Space(6);
        GUILayout.Label("Zustands-Flags", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        DrawBoolRow("Hungrig",         info.IsHungry);
        DrawBoolRow("Durstig",         info.IsThirsty);
        DrawBoolRow("Müde",            info.IsTired);
        DrawBoolRow("Bereit zu maten", info.IsReadyToMate);
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(4);
    }

    private void DrawNeedBar(string label, float value, bool isWarning)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.Width(70));
        var prev = GUI.color;
        GUI.color = isWarning ? new Color(1f, 0.4f, 0.3f) : new Color(0.4f, 0.85f, 0.5f);
        var rect = GUILayoutUtility.GetRect(0, 16, GUILayout.ExpandWidth(true));
        EditorGUI.ProgressBar(rect, value / 100f, $"{value:F1}");
        GUI.color = prev;
        EditorGUILayout.EndHorizontal();
    }

    private void DrawBoolRow(string label, bool value)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.Width(220));
        var prev = GUI.color;
        GUI.color = value ? new Color(0.4f, 0.9f, 0.5f) : new Color(0.6f, 0.6f, 0.6f);
        GUILayout.Label(value ? "✔  ja" : "✘  nein", EditorStyles.boldLabel);
        GUI.color = prev;
        EditorGUILayout.EndHorizontal();
    }

    private void DrawStateRow(string label, string stateName, Color color)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.Width(130));
        var prev = GUI.color;
        GUI.color = color;
        GUILayout.Label(stateName, EditorStyles.boldLabel);
        GUI.color = prev;
        EditorGUILayout.EndHorizontal();
    }

    private Color StateColor(string s) => s switch
    {
        "FleeState"            => new Color(1f,   0.3f, 0.3f),
        "EatState"             => new Color(0.4f, 0.9f, 0.4f),
        "SeekFoodState"        => new Color(0.7f, 0.9f, 0.4f),
        "SeekMemoryFoodState"  => new Color(0.9f, 0.75f, 0.2f),  // amber — searching from memory
        "DrinkState"           => new Color(0.3f, 0.7f, 1f),
        "SeekWaterState"       => new Color(0.5f, 0.8f, 1f),
        "SeekMemoryWaterState" => new Color(0.6f, 0.85f, 1f),    // light blue — memory water
        "SleepState"           => new Color(0.7f, 0.6f, 1f),
        "GoHomeState"          => new Color(0.9f, 0.8f, 0.4f),
        "ReproduceState"       => new Color(1f,   0.6f, 0.8f),
        "IdleState"            => new Color(0.8f, 0.8f, 0.8f),
        _                      => Color.white
    };
}