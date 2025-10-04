using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsSceneHandlerScript : MonoBehaviour
{
    public string settingsSceneName = "Settings";
    public string returnSceneName = "map";

    // Wird vom Settings-Button aufgerufen
    public void OpenSettings()
    {
        var gdm = FindObjectOfType<GameDataManager>();
        if (gdm != null)
        {
            gdm.SaveGame();
            Debug.Log("Vor Settings: Spiel gespeichert.");
        }
        else
        {
            Debug.LogWarning("GameDataManager nicht gefunden - konnte nicht speichern!");
        }

        SceneManager.LoadScene(settingsSceneName);
    }

    // Wird im Settings-Menü "Zurück" Button aufgerufen
    public void CloseSettingsAndReturn()
    {
        // Nach dem Laden die Szene-Callback abonnieren, damit LoadGame wirklich nach dem Scene-Load läuft
        SceneManager.sceneLoaded += OnSceneLoadedAfterReturn;
        SceneManager.LoadScene(returnSceneName);
    }

    private void OnSceneLoadedAfterReturn(Scene scene, LoadSceneMode mode)
    {
        // Unsubscribe
        SceneManager.sceneLoaded -= OnSceneLoadedAfterReturn;

        // Versuchen zu laden
        var gdm = FindObjectOfType<GameDataManager>();
        if (gdm != null)
        {
            gdm.LoadGame();
            Debug.Log("Nach Rückkehr: Spiel geladen.");
        }
        else
        {
            Debug.LogWarning("GameDataManager nach Rückkehr nicht gefunden. Stelle sicher, dass GameDataManager in der Map-Szene vorhanden ist.");
        }
    }
}
