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


    public Transform rightHandHold;
    public Transform leftHandHold;
    private GameObject rightHandItem;
    private GameObject leftHandItem;
    public string rightHandItemSave;
    public string leftHandItemSave;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        hungerSlider.maxValue = 100;
        hungerSlider.value = hunger;
        thirstSlider.maxValue = 100;
        thirstSlider.value = thirst;

        interactionText = interactionTextObject.GetComponent<TMP_Text>();
        interactionText.text = "";

        FindObjectOfType<GameDataManager>().LoadGame();
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
            interactionText.text = "press E to pick up " + nearbyItem.itemName;
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

        var cave = other.GetComponent<CaveScript>();
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
        { "Hammerstone+Hammerstone", "Stonesplinter" }

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

            SpawnInRightHand(result);
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
        }

        if (prefab == null)
        {
            Debug.LogWarning($"Kein Prefab für {itemName} gesetzt!");
            return;
        }

        var handItem = Instantiate(prefab, rightHandHold.position, rightHandHold.rotation);
        handItem.tag = "Item" + itemName;
        handItem.transform.SetParent(rightHandHold);
        //handItem.transform.localScale = Vector3.one * 0.5f;

        rightHandItem = handItem;
        rightHandItemSave = itemName;
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
            default: return;
        }

        if (prefab != null)
        {
            var handItem = Instantiate(prefab, handHold.position, handHold.rotation);
            handItem.tag = "Item" + itemName;
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
