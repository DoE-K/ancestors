using UnityEngine;

public class CaveSceneManager : MonoBehaviour
{
    [System.Serializable]
    public class CaveSpawn
    {
        public string caveName;
        public Transform spawnPoint;
    }

    public CaveSpawn[] caveSpawns;

    void Start()
    {
        string caveName = PlayerPrefs.GetString("CaveName", "");

        if (string.IsNullOrEmpty(caveName))
        {
            Debug.LogWarning("Keine CaveName gefunden – Standardposition wird verwendet.");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Kein Player gefunden in der CaveScene!");
            return;
        }

        foreach (var cave in caveSpawns)
        {
            if (cave.caveName == caveName)
            {
                player.transform.position = cave.spawnPoint.position;
                Debug.Log($"Spieler wurde zu {caveName} teleportiert.");
                return;
            }
        }

        Debug.LogWarning($"Kein Spawnpunkt für {caveName} gefunden.");
    }
}
