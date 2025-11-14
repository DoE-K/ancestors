using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class PlayerScript : MonoBehaviour
{
    
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    
    public Slider hungerSlider;
    public Slider thirstSlider;

    public float hunger = 100f;
    public float thirst = 100f;
    private float hungerDecayRate = 0.028f;
    private float thirstDecayRate = 0.083f;

    private ItemScript nearbyItem;
    private FoodScript nearbyFood;
    private WaterScript nearbyWater;

    public GameObject interactionTextObject;
    private TMP_Text interactionText;

    //ITEMS
    public GameObject stoneItemPrfab;
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

    [Header("Spawn Points")]
    public Transform raftSpawnPoint;
    public Transform boatSpawnPoint;
    public Transform shipSpawnPoint;

    // Set speichert, was schon gespawnt wurde
    private HashSet<string> spawnedItems = new HashSet<string>();


    public Transform rightHandHold;
    public Transform leftHandHold;
    private GameObject rightHandItem;
    private GameObject leftHandItem;
    public string rightHandItemSave;
    public string leftHandItemSave;

    public HighscoreScript hss;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        hungerSlider.maxValue = 100;
        hungerSlider.value = hunger;
        thirstSlider.maxValue = 100;
        thirstSlider.value = thirst;

        interactionText = interactionTextObject.GetComponent<TMP_Text>();
        interactionText.text = "";

        //FindObjectOfType<GameDataManager>().LoadGame();
        GlobalScore.score = 0;
    }

    void Update()
    {
        
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        
        hunger -= hungerDecayRate * Time.deltaTime;
        thirst -= thirstDecayRate * Time.deltaTime;

        hunger = Mathf.Clamp(hunger, 0f, 100f);
        thirst = Mathf.Clamp(thirst, 0f, 100f);

        hungerSlider.value = hunger;
        thirstSlider.value = thirst;

        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("lvl1 bin drin");
            // Prüfe rechte Hand
            if (rightHandItem != null && rightHandItem.CompareTag("Food"))
            {
                Debug.Log("lvl2 bin drin");
                var food = rightHandItem.GetComponent<FoodScript>();
                if (food != null)
                {
                    float nutrition = food.nutrition * rightHandItem.transform.localScale.magnitude;
                    EatFood(nutrition);
                    Destroy(rightHandItem);
                    rightHandItem = null;
                    rightHandItemSave = "";
                    Debug.Log($"Food gegessen! Nutrition: {nutrition:F1}");
                }

            }

            // Prüfe linke Hand
            else if (leftHandItem != null && leftHandItem.CompareTag("Food"))
            {
                Debug.Log("lvl3 bin drin");
                var food = leftHandItem.GetComponent<FoodScript>();
                if (food != null)
                {
                    float nutrition = food.nutrition * leftHandItem.transform.localScale.magnitude;
                    EatFood(nutrition);
                    Destroy(leftHandItem);
                    leftHandItem = null;
                    leftHandItemSave = "";
                    Debug.Log($"Food gegessen! Nutrition: {nutrition:F1}");
                }

            }
        }

        if (nearbyWater != null)
        {
            interactionText.text = "press R to drink";
            if (Input.GetKeyDown(KeyCode.R))
            {
                DrinkWater(nearbyWater.thirst);
            }
        }
        if (nearbyItem != null)
        {
            if(nearbyItem.AbleToPickup)
            {
                interactionText.text = "press E to pick up " + nearbyItem.itemName;
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpItem(nearbyItem);
                nearbyItem = null; // nach Aufnahme zurücksetzen
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
            Debug.Log("C wurde gedrückt!");
            Craft();
        }


        
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveSystem.ResetSave();
        }

        if (hunger <= 0 || thirst <= 0) Die();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemScript item = other.GetComponent<ItemScript>();
        if (item != null)
        {
            nearbyItem = item;
        }

        if (other.CompareTag("Food"))
        {
            nearbyFood = other.GetComponent<FoodScript>();
        }

        if (other.CompareTag("Water"))
        {
            nearbyWater = other.GetComponent<WaterScript>();
        }

        /*var cave = other.GetComponent<CaveScript>();
        if (cave != null)
        {
            FindObjectOfType<GameDataManager>().SaveGame();

            if (!cave.isExit)
            {
                Debug.Log($"Betrete Höhle: {cave.caveName}");
                SceneManager.LoadScene(cave.caveName);
            }
            else
            {
                Debug.Log($"Verlasse Höhle: {cave.caveName}");
                SceneManager.LoadScene("map");
            }
        }*/

        //var cave = other.GetComponent<CaveScript>();

        if(other.CompareTag("cave"))
        {
            var cave = other.GetComponent<CaveScript>();

            if(!cave.isExit)
            {
                if(cave.caveName == "cave0")
                {
                    Debug.Log("teleport to cave0");
                    transform.position = new Vector3 (0, -156, 0);
                }

                if(cave.caveName == "cave1")
                {
                    Debug.Log("teleport to cave1");
                    transform.position = new Vector3 (-90, -202, 0);
                }

                if(cave.caveName == "cave2")
                {
                    Debug.Log("teleport to cave2");
                    transform.position = new Vector3 (-185, -202, 0);
                }

                if(cave.caveName == "cave3")
                {
                    Debug.Log("teleport to cave3");
                    transform.position = new Vector3 (-275, -202, 0);
                }
            }
            else
            {
                if(cave.caveName == "cave0")
                {
                    Debug.Log("teleport to cave0!");
                    transform.position = new Vector3 (-45, 28, 0);
                }

                if(cave.caveName == "cave1")
                {
                    Debug.Log("teleport to cave1!");
                    transform.position = new Vector3 (-200, 73, 0);
                }

                if(cave.caveName == "cave2")
                {
                    Debug.Log("teleport to cave2!");
                    transform.position = new Vector3 (-105, -42, 0);
                }

                if(cave.caveName == "cave3")
                {
                    Debug.Log("teleport to cave3!");
                    transform.position = new Vector3 (20, 78, 0);
                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ItemScript item = other.GetComponent<ItemScript>();
        if (item != null && item == nearbyItem)
        {
            nearbyItem = null;
        }

        if (other.CompareTag("Food"))
        {
            if (other.GetComponent<FoodScript>() == nearbyFood)
            {
                nearbyFood = null;
                interactionText.text = "";
            }
        }

        if (other.CompareTag("Water"))
        {
            if (other.GetComponent<WaterScript>() == nearbyWater)
            {
                nearbyWater = null;
                interactionText.text = "";
            }
        }
    }

    public void EatFood(float amount)
    {
        hunger = Mathf.Clamp(hunger + amount, 0f, 100f);
        hungerSlider.value = hunger;
    }

    public void DrinkWater(float amount)
    {
        thirst = Mathf.Clamp(thirst + amount, 0f, 100f);
        thirstSlider.value = thirst;
    }

    // High-Level Version: nur 4 Parameter
    void PickUpItem(ItemScript item)
    {
        if (rightHandItem == null)
        {
            PickUpItemInternal(ref rightHandItem, ref rightHandItemSave, rightHandHold, item);
            //GameDataManager.SaveGame();
        }
        else if (leftHandItem == null)
        {
            PickUpItemInternal(ref leftHandItem, ref leftHandItemSave, leftHandHold, item);
            //GameDataManager.SaveGame();
        }
    }

    // Low-Level Version: ausführlich
    void PickUpItemInternal(ref GameObject handSlot, ref string handSave, Transform handHold, ItemScript item)
    {
        if(item.AbleToPickup)
        {
            // In die Hand legen
            handSlot = Instantiate(item.prefab, handHold.position, handHold.rotation);
            handSlot.transform.SetParent(handHold);
            handSave = item.itemName;
            Debug.Log($"Aufgehoben: {item.itemName}");


            // Quelle entfernen oder nicht
            if (item.destroyOnPickup)
            {
                Destroy(item.gameObject);
            }
            else
            {
                Debug.Log($"{item.itemName} wurde entnommen, Quelle bleibt bestehen.");
                // optional: cooldown oder drop counter einbauen
            }
        }
        
    }




    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    // ------------------------------------
    // Rezepte definieren
    // ------------------------------------
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

        { "Hide+Fire", "Driedhire" },  
        { "Driedhide+Obsidianblade", "Preparedhide" },
        { "Cordage+Plantfiber", "Fabric" },

        { "Cordage+Fabric", "Sail" }, 
        { "Cordage+Plank", "Raft" }, 
        { "Plank+Raftblueprint", "Boat" }, //eigentlich mit sail aber später
        { "Boatblueprint+Plank", "Ship" }

    };


    // ------------------------------------
    // Craft-Funktion
    // ------------------------------------
    void Craft()
    {
        Debug.Log($"Links: {leftHandItemSave}, Rechts: {rightHandItemSave}");
        if (string.IsNullOrEmpty(leftHandItemSave) || string.IsNullOrEmpty(rightHandItemSave))
            return;

        // Schlüssel erstellen (alphabetisch sortieren, damit Reihenfolge egal ist)
        List<string> items = new List<string> { leftHandItemSave, rightHandItemSave };
        items.Sort();
        string key = string.Join("+", items);

        if (craftingRecipes.TryGetValue(key, out string result))
        {
            Debug.Log($"Crafting {result}!");
            Destroy(leftHandItem);
            Destroy(rightHandItem);
            leftHandItem = null;
            rightHandItem = null;
            leftHandItemSave = "";
            rightHandItemSave = "";

            if (result == "Raft")
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
            }
        }
        else
        {
            Debug.Log("Nicht craftbar");
        }
    }


    // ------------------------------------
    // Hilfsmethode zum Spawnen in der rechten Hand
    // ------------------------------------
    void SpawnInRightHand(string itemName)
    {
        Debug.Log($"SpawnInRightHand CALLED WITH: {itemName}");

        GameObject prefab = null;

        switch (itemName)
        {
            case "Stone": prefab = stoneItemPrfab; break;
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
            case "UrBlackberry": prefab = urblackberryItemPrefab; break;
            case "UrBlueberry": prefab = urblueberryItemPrefab; break;
            case "UrFig": prefab = urfigItemPrefab; break;
            case "UrMango": prefab = urmangoItemPrefab; break;
            case "UrDate": prefab = urdateItemPrefab; break; 
            case "UrAvocado": prefab = uravocadoItemPrefab; break;
            case "Bone": prefab = boneItemPrefab; break;
            case "Boneshard": prefab = boneshardItemPrefab; break;
            case "Needle": prefab = needleItemPrefab; break;
            case "Hide": prefab = hideItemPrefab; break;
            case "Dried": prefab = driedhideItemPrefab; break;
            case "Preparedhide": prefab = preparedhideItemPrefab; break;
            case "Fabric": prefab = fabricItemPrefab; break;
            case "Plank": prefab = plankItemPrefab; break;
            case "Raft": prefab = raftItemPrefab; break;
            case "Raftblueprint": prefab = raftblueprintItemPrefab; break;
            case "Boat": prefab = boatItemPrefab; break;
            case "Boatblueprint": prefab = boatblueprintItemPrefab; break;
            case "Ship": prefab = shipItemPrefab; break;
            case "Shipblueprint": prefab = shipblueprintItemPrefab; break;
        }

        if (prefab == null)
        {
            Debug.LogWarning($"Kein Prefab für {itemName} gesetzt!");
            return;
        }

        var handItem = Instantiate(prefab, rightHandHold.position, rightHandHold.rotation);
        //handItem.tag = "Item" + itemName;
        handItem.transform.SetParent(rightHandHold);
        //handItem.transform.localScale = Vector3.one * 0.5f;

        rightHandItem = handItem;
        rightHandItemSave = itemName;

        addScore(10);
    }

    void SpawnInMap(string itemName)
    {
        Debug.Log($"SpawnInMap aufgerufen mit {itemName}");

        // Überprüfen, ob dieses Item schon gespawnt wurde
        if (spawnedItems.Contains(itemName))
        {
            Debug.LogWarning($"{itemName} wurde bereits gecrafted – Spawn wird abgebrochen!");
            return;
        }

        GameObject prefab = null;
        Transform spawnPoint = null;

        // Prefab und Spawnpoint anhand des Namens auswählen
        switch (itemName)
        {
            case "Raft":
                prefab = raftItemPrefab;
                spawnPoint = raftSpawnPoint;
                addScore(20);
                break;
            case "Boat":
                prefab = boatItemPrefab;
                spawnPoint = boatSpawnPoint;
                addScore(30);
                break;
            case "Ship":
                prefab = shipItemPrefab;
                spawnPoint = shipSpawnPoint;
                addScore(40);
                break;
            default:
                Debug.LogWarning($"Unbekannter Itemname '{itemName}' in SpawnInMap().");
                return;
        }

        // Sicherstellen, dass alles korrekt gesetzt ist
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

        // Item in der Welt spawnen
        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        spawnedItems.Add(itemName);

        Debug.Log($"{itemName} erfolgreich am Strand gespawnt!");
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void DropItem(GameObject item, ref GameObject slot, string prefabName)
    {
        if (item != null)
        {
            item.transform.SetParent(null);

            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb == null) rb = item.AddComponent<Rigidbody>();

            Collider col = item.GetComponent<Collider>();
            if (col != null) col.enabled = true;

            MarkItem(item, prefabName);
            slot = null;
        }
    }

    public void RestoreItem(string itemName, bool isRightHand)
    {
        Debug.Log("Moin Moin");
        Transform handHold = isRightHand ? rightHandHold : leftHandHold;
        GameObject prefab = null;

        switch (itemName)
        {
            case "Stone": prefab = stoneItemPrfab; break;
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
            case "Bone": prefab = boneItemPrefab; break;
            case "Boneshard": prefab = boneshardItemPrefab; break;
            case "Needle": prefab = needleItemPrefab; break;
            case "Hide": prefab = hideItemPrefab; break;
            case "Dried": prefab = driedhideItemPrefab; break;
            case "Preparedhide": prefab = preparedhideItemPrefab; break;
            case "Fabric": prefab = fabricItemPrefab; break;
            case "Plank": prefab = plankItemPrefab; break;
            case "Raft": prefab = raftItemPrefab; break;
            case "Raftblueprint": prefab = raftblueprintItemPrefab; break;
            case "Boat": prefab = boatItemPrefab; break;
            case "Boatblueprint": prefab = boatblueprintItemPrefab; break;
            case "Ship": prefab = shipItemPrefab; break;
            case "Shipblueprint": prefab = shipblueprintItemPrefab; break;
            default: return;
        }

        if (prefab != null)
        {
            var handItem = Instantiate(prefab, handHold.position, handHold.rotation);
            //handItem.tag = "Item" + itemName;
            handItem.transform.SetParent(handHold);
            //handItem.transform.localScale = Vector3.one * 0.5f;

            if (isRightHand)
            {
                rightHandItem = handItem;
                rightHandItemSave = itemName;
            }
            else
            {
                leftHandItem = handItem;
                leftHandItemSave = itemName;
            }
        }
    }

    void addScore(int points)
    {
        //hss.score = hss.score + points;
        //PlayerPrefs.SetInt("Score", hss.score);

        GlobalScore.AddScore(points);
    }

    void MarkItem(GameObject item, string prefabName)
    {
        var marker = item.AddComponent<WorldItemMarker>();
        marker.prefabName = prefabName;
    }

    void Die()
    {
        Debug.Log("Spieler ist gestorben!");
    }
}
