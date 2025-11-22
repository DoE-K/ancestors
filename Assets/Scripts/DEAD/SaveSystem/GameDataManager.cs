/*using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameDataManager : MonoBehaviour
{
    public PlayerScript player;
    public WorldDataManager worldManager;

    public void SaveGame()
    {
        PlayerData data = new PlayerData();

        // Grunddaten
        data.hunger = player.hunger;
        data.thirst = player.thirst;
        data.rightHandItemSave = player.rightHandItemSave;
        data.leftHandItemSave = player.leftHandItemSave;

        // ✅ Szenenname + Spielerposition speichern
        string currentScene = SceneManager.GetActiveScene().name;
        Vector3 pos = player.transform.position;

        ScenePositionData existing = data.scenePositions.Find(s => s.sceneName == currentScene);
        if (existing != null)
            existing.position = new float[] { pos.x, pos.y, pos.z };
        else
            data.scenePositions.Add(new ScenePositionData { sceneName = currentScene, position = new float[] { pos.x, pos.y, pos.z } });

        // Weltobjekte speichern
        WorldItemMarker[] itemsInWorld = FindObjectsOfType<WorldItemMarker>();
        foreach (var marker in itemsInWorld)
        {
            WorldItemData itemData = new WorldItemData();
            itemData.prefabName = marker.prefabName;
            itemData.posX = marker.transform.position.x;
            itemData.posY = marker.transform.position.y;
            itemData.posZ = marker.transform.position.z;
            itemData.rotX = marker.transform.eulerAngles.x;
            itemData.rotY = marker.transform.eulerAngles.y;
            itemData.rotZ = marker.transform.eulerAngles.z;

            data.worldItems.Add(itemData);
        }

        SaveSystem.Save(data);
        Debug.Log($"✅ Spiel gespeichert (Szene: {currentScene}, Pos: {pos})");
    }

    public void LoadGame()
    {
        PlayerData data = SaveSystem.Load();
        if (data != null)
        {
            player.hunger = data.hunger;
            player.thirst = data.thirst;

            // ✅ Spielerposition für aktuelle Szene laden
            string currentScene = SceneManager.GetActiveScene().name;
            ScenePositionData sceneData = data.scenePositions.Find(s => s.sceneName == currentScene);

            if (sceneData != null && sceneData.position.Length == 3)
            {
                player.transform.position = new Vector3(sceneData.position[0], sceneData.position[1], sceneData.position[2]);
                Debug.Log($"✅ Spielerposition für Szene '{currentScene}' geladen: {sceneData.position[0]}, {sceneData.position[1]}, {sceneData.position[2]}");
            }
            else
            {
                Debug.Log($"ℹ️ Keine gespeicherte Position für Szene '{currentScene}' gefunden, Standard bleibt.");
            }

            // Hände wiederherstellen
            if (!string.IsNullOrEmpty(data.rightHandItemSave))
                player.RestoreItem(data.rightHandItemSave, true);
            if (!string.IsNullOrEmpty(data.leftHandItemSave))
                player.RestoreItem(data.leftHandItemSave, false);

            // Weltobjekte laden
            worldManager.RestoreWorldItems(data.worldItems);
        }
    }
}*/
