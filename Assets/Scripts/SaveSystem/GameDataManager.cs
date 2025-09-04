using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public PlayerScript player;
    public WorldDataManager worldManager;

    public void SaveGame()
    {
        PlayerData data = new PlayerData();

        data.hunger = player.hunger;
        data.thirst = player.thirst;
        data.rightHandItemSave = player.rightHandItemSave;
        data.leftHandItemSave = player.leftHandItemSave;

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
    }

    public void LoadGame()
    {
        PlayerData data = SaveSystem.Load();
        if (data != null)
        {
            player.hunger = data.hunger;
            player.thirst = data.thirst;
            player.rightHandItemSave = data.rightHandItemSave;
            player.leftHandItemSave = data.leftHandItemSave;

            worldManager.RestoreWorldItems(data.worldItems);
        }
    }


    
}


