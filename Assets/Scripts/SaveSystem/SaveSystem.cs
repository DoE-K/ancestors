using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/save.json";

    public static void Save(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log($"Spiel gespeichert unter: {path}");
    }

    public static PlayerData Load()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Savegame geladen!");
            return data;
        }
        else
        {
            Debug.LogWarning("Keine Speicherdatei gefunden.");
            return null;
        }
    }

    public static void ResetSave()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Savegame wurde zurückgesetzt!");
        }
    }
}
