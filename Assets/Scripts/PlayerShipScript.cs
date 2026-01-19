using UnityEngine;

public class PlayerShipScript : MonoBehaviour
{
    public GameObject SceneShip;

    
    void Start()
    {
        
    }

    void Update()
    {
        if(GlobalScore.score >= 100)
        {
            SpawnInMap("Ship");
            Debug.Log("Ship spawned in Map");
        }
    }

    void SpawnInMap(string itemName)
    {
        /*Debug.Log($"SpawnInMap aufgerufen mit {itemName}");

        if (spawnedItems.Contains(itemName))
        {
            return;
        }

        GameObject prefab = null;
        Transform spawnPoint = null;

        switch (itemName)
        {
            case "Raft":
                SceneRaft.SetActive(true);
                break;
            case "Boat":
                SceneBoat.SetActive(true);
                break;
            case "Ship":
                SceneShip.SetActive(true);
                break;
            default:
                Debug.LogWarning($"Unbekannter Itemname '{itemName}' in SpawnInMap().");
                return;
        }

        if (prefab == null)
        {
            Debug.LogWarning($"Kein Prefab für {itemName} gesetzt!");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogWarning($"Kein Spawnpoint für {itemName} gesetzt!");
            return;
        }

        spawnedItems.Add(itemName);

        Debug.Log($"{itemName} erfolgreich am Strand gespawnt!");*/

        SceneShip.SetActive(true);
    }
}
