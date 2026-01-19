using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class PlayerScript : MonoBehaviour
{

    private ItemScript nearbyItem;
    //private FoodScript nearbyFood;
    //private WaterScript nearbyWater;

    //public GameObject interactionTextObject;
    public TMP_Text interactionText;

    [Header("Items")]
    public GameObject stoneItemPrfab;
    public GameObject branchItemPrefab;
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
    public GameObject urblackberryItemPrefab;
    public GameObject urblueberryItemPrefab;
    public GameObject urfigItemPrefab;
    public GameObject urmangoItemPrefab;
    public GameObject urdateItemPrefab;
    public GameObject uravocadoItemPrefab;
    public GameObject boneItemPrefab;
    public GameObject boneshardItemPrefab;
    public GameObject needleItemPrefab;
    public GameObject hideItemPrefab;
    public GameObject driedhideItemPrefab;
    public GameObject preparedhideItemPrefab;
    public GameObject fabricItemPrefab;
    public GameObject plankItemPrefab;
    public GameObject raftItemPrefab;
    public GameObject raftblueprintItemPrefab;
    public GameObject boatItemPrefab;
    public GameObject boatblueprintItemPrefab;
    public GameObject shipItemPrefab;
    public GameObject shipblueprintItemPrefab;
    public GameObject SceneRaft;
    public GameObject SceneBoat;
    public GameObject SceneShip;

    [Header("Spawn Points")]
    public Transform raftSpawnPoint;
    public Transform boatSpawnPoint;
    public Transform shipSpawnPoint;

    //private HashSet<string> spawnedItems = new HashSet<string>();

    public Transform rightHandHold;
    public Transform leftHandHold;
    private GameObject rightHandItem;
    private GameObject leftHandItem;
    public string rightHandItemSave;
    public string leftHandItemSave;

    public HighscoreScript hss;

    void Start()
    {

        //interactionText = interactionTextObject.GetComponent<TMP_Text>();
        interactionText.text = "";

        GlobalScore.score = 0; //THE SCENE SHOULD DO THAT
    }

    void Update()
    {

        

        

        if (nearbyItem != null)
        {
            if(nearbyItem.AbleToPickup)
            {
                interactionText.text = nearbyItem.itemName;
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpItem(nearbyItem);
                nearbyItem = null;
            }
        }
        else
        {
            interactionText.text = "";
        }

        
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItem(rightHandItem, ref rightHandItem, rightHandItemSave);
            rightHandItemSave = "";
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            DropItem(leftHandItem, ref leftHandItem, leftHandItemSave);
            leftHandItemSave = "";
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Craft();
        }

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemScript item = other.GetComponent<ItemScript>();
        if (item != null)
        {
            nearbyItem = item;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ItemScript item = other.GetComponent<ItemScript>();
        if (item != null && item == nearbyItem)
        {
            nearbyItem = null;
        }
    }

    void PickUpItem(ItemScript item)
    {
        if (rightHandItem == null)
        {
            PickUpItemInternal(ref rightHandItem, ref rightHandItemSave, rightHandHold, item);
            //SpawnInRightHand(item); CLEANING TO DO
        }
        else if (leftHandItem == null)
        {
            PickUpItemInternal(ref leftHandItem, ref leftHandItemSave, leftHandHold, item);
        }
    }

    void PickUpItemInternal(ref GameObject handSlot, ref string handSave, Transform handHold, ItemScript item)
    {
        if(item.AbleToPickup)
        {
            handSlot = Instantiate(item.prefab, handHold.position, handHold.rotation);
            handSlot.transform.SetParent(handHold);
            handSave = item.itemName;

            if (item.destroyOnPickup)
            {
                Destroy(item.gameObject);
            }
            else
            {
                Debug.Log($"{item.itemName} wurde entnommen, Quelle bleibt bestehen.");
            }
        }
        
    }

    private Dictionary<string, string> craftingRecipes = new Dictionary<string, string>
    {
        { "Branch+Stone", "Fire" },
        { "Stone+Stone", "Hammerstone" },
        { "Branch+Hammerstone", "Stick" },
        { "Stick+Stick", "Woodpiece" },
        { "Hammerstone+Stone", "Stonesplinter" },
        { "Hammerstone+Obsidian", "Obsidiansplinter" },
        { "Stick+Stonesplinter", "Stoneblade" },
        { "Obsidiansplinter+Stick", "Obsidianblade" }, 
        { "Plantfiber+Plantfiber", "Cordage" },

        { "Woodpiece+Woodpiece", "Plank" },

        { "Bone+Hammerstone", "Boneshard" }, 
        { "Boneshard+Obsidianblade", "Needle" },

        { "Fire+Hide", "Driedhide" },  
        { "Driedhide+Obsidianblade", "Preparedhide" },
        { "Needle+Plantfiber", "Fabric" },

        //{ "Cordage+Fabric", "Sail" }, 'OLD'
        //{ "Cordage+Plank", "Raft" }, 'OLD'
        //{ "Fabric+Raftblueprint", "Boat" }, 'OLD'
        //{ "Plank+Cordage", "Ship" } 'OLD'

    };

    void Craft()
    {
        Debug.Log($"Links: {leftHandItemSave}, Rechts: {rightHandItemSave}");
        if (string.IsNullOrEmpty(leftHandItemSave) || string.IsNullOrEmpty(rightHandItemSave))
            return;

        List<string> items = new List<string> { leftHandItemSave, rightHandItemSave };
        items.Sort();
        string key = string.Join("+", items);

        if (craftingRecipes.TryGetValue(key, out string result))
        {
            Destroy(leftHandItem);
            Destroy(rightHandItem);
            leftHandItem = null;
            rightHandItem = null;
            leftHandItemSave = "";
            rightHandItemSave = "";

            /*if (result == "Raft")
            {
                SpawnInMap(result);
                SpawnInRightHand("Raftblueprint");
            }
            else if (result == "Boat")
            {
                SpawnInMap(result);
                SpawnInRightHand("Boatblueprint");
            }
            else if (result == "Ship")
            {
                SpawnInMap(result);
                SpawnInRightHand("Shipblueprint");
            }
            else
            {
                SpawnInRightHand(result);
            }*/
        }
        else
        {
            Debug.Log("Nicht craftbar"); //TO DO
        }
    }

    void SpawnInRightHand(string itemName)
    {
        Debug.Log($"SpawnInRightHand CALLED WITH: {itemName}");

        GameObject prefab = null;

        switch (itemName)
        {
            case "Stone": prefab = stoneItemPrfab; break;
            case "Branch": prefab = branchItemPrefab; break;
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
            case "AncientBlackberry": prefab = urblackberryItemPrefab; break;
            case "AncientBlueberry": prefab = urblueberryItemPrefab; break;
            case "AncientFig": prefab = urfigItemPrefab; break;
            case "AncientMango": prefab = urmangoItemPrefab; break;
            case "AncientDate": prefab = urdateItemPrefab; break; 
            case "AncientAvocado": prefab = uravocadoItemPrefab; break;
            case "Bone": prefab = boneItemPrefab; break;
            case "Boneshard": prefab = boneshardItemPrefab; break;
            case "Needle": prefab = needleItemPrefab; break;
            case "Hide": prefab = hideItemPrefab; break;
            case "Driedhide": prefab = driedhideItemPrefab; break;
            case "Preparedhide": prefab = preparedhideItemPrefab; break;
            case "Fabric": prefab = fabricItemPrefab; break;
            case "Plank": prefab = plankItemPrefab; break;
            //case "Raft": prefab = raftItemPrefab; break;
            //case "Raftblueprint": prefab = raftblueprintItemPrefab; break;
            //case "Boat": prefab = boatItemPrefab; break;
            //case "Boatblueprint": prefab = boatblueprintItemPrefab; break;
            case "Ship": prefab = shipItemPrefab; break;
            //case "Shipblueprint": prefab = shipblueprintItemPrefab; break;
        }

        if (prefab == null)
        {
            Debug.LogWarning($"Kein Prefab für {itemName} gesetzt!"); //TO DO
            return;
        }

        var handItem = Instantiate(prefab, rightHandHold.position, rightHandHold.rotation);
        handItem.transform.SetParent(rightHandHold);

        rightHandItem = handItem;
        rightHandItemSave = itemName;
    }

    void DropItem(GameObject item, ref GameObject slot, string prefabName)
    {
        if (item != null)
        {
            item.transform.SetParent(null);

            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb == null) rb = item.AddComponent<Rigidbody>();

            Collider col = item.GetComponent<Collider>();
            if (col != null) col.enabled = true;

            //MarkItem(item, prefabName);
            slot = null;
        }
    }

}
