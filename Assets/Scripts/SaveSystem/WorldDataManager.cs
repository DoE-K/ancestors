using UnityEngine;
using System.Collections.Generic;

public class WorldDataManager : MonoBehaviour
{
    public GameObject stoneItemPrefab;
    public GameObject branchItemPrefab;
    public GameObject silverItemPrefab;
    public GameObject goldItemPrefab;
    public GameObject fireItemPrefab;
    public GameObject cordageItemPrefab;
    public GameObject hammerstoneItemPrefab;
    public GameObject obsidianItemPrefab;
    public GameObject obsidianbladeItemPrefab;
    public GameObject obsidiansplinterItemPrefab;
    public GameObject plantfiberItemPrefab;
    public GameObject stickItemPrefab;
    public GameObject stonebladeItemPrefab;
    public GameObject stonesplinterItemPrefab;
    public GameObject woodpieceItemPrefab;
    public GameObject ficusItemPrefab;
    public GameObject rubusItemPrefab;
    public GameObject vaccItemPrefab;

    public void RestoreWorldItems(List<WorldItemData> items)
    {
        foreach (var itemData in items)
        {
            GameObject prefab = null;
            switch (itemData.prefabName)
            {
                case "Stone": prefab = stoneItemPrefab; break;
                case "Branch": prefab = branchItemPrefab; break;
                case "Silver": prefab = silverItemPrefab; break;
                case "Gold": prefab = goldItemPrefab; break;
                case "Fire": prefab = fireItemPrefab; break;
                case "Cordage": prefab = cordageItemPrefab; break;
                case "Hammerstone": prefab = hammerstoneItemPrefab; break;
                case "Obsidian": prefab = obsidianItemPrefab; break;
                case "Obsidianblade": prefab = obsidianbladeItemPrefab; break;
                case "Obsidiansplinter": prefab = obsidiansplinterItemPrefab; break;
                case "Plantfiber": prefab = plantfiberItemPrefab; break;
                case "Stick": prefab = stickItemPrefab; break;
                case "Stoneblade": prefab = stonebladeItemPrefab; break;
                case "Stonesplinter": prefab = stonesplinterItemPrefab; break;
                case "Woodpiece": prefab = woodpieceItemPrefab; break;
                case "Ficus": prefab = ficusItemPrefab; break;
                case "Rubus": prefab = rubusItemPrefab; break;
                case "Vacc": prefab = vaccItemPrefab; break; 
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

    private void MarkItem(GameObject item, string prefabName)
    {
        var marker = item.AddComponent<WorldItemMarker>();
        marker.prefabName = prefabName;
    }
}
