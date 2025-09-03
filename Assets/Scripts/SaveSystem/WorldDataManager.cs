using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameObject stonePrefab;
    public GameObject treePrefab;
    public GameObject silverPrefab;
    public GameObject goldPrefab;

    public void RestoreWorldItems(List<WorldItemData> items)
    {
        foreach (var itemData in items)
        {
            GameObject prefab = null;
            switch (itemData.prefabName)
            {
                case "Stone": prefab = stonePrefab; break;
                case "Tree": prefab = treePrefab; break;
                case "Silver": prefab = silverPrefab; break;
                case "Gold": prefab = goldPrefab; break;
            }

            if (prefab != null)
            {
                GameObject obj = Instantiate(
                    prefab,
                    new Vector3(itemData.posX, itemData.posY, itemData.posZ),
                    Quaternion.Euler(itemData.rotX, itemData.rotY, itemData.rotZ)
                );

                MarkItem(obj, itemData.prefabName);
            }
        }
    }
}
