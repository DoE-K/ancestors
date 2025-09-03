using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    // Movement
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    // Hunger/Durst/UI
    public Slider hungerSlider;
    public Slider thirstSlider;

    private float hunger = 100f;
    private float thirst = 100f;
    private float hungerDecayRate = 0.028f;
    private float thirstDecayRate = 0.083f;

    private FoodScript nearbyFood;
    private WaterScript nearbyWater;
    private StoneScript nearbyStone;
    private TreeScript nearbyTree;
    private SilverScript nearbySilver;
    private GoldScript nearbyGold;


    public GameObject interactionTextObject;
    private TMP_Text interactionText;

    public Transform rightHandHold;
    public Transform leftHandHold;
    public GameObject stonePrefab;
    public GameObject treePrefab;
    public GameObject silverPrefab;
    public GameObject goldPrefab;

    private GameObject rightHandItem;
    private GameObject leftHandItem;
    private string rightHandItemSave;
    private string leftHandItemSave;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // <- wichtig!

        hungerSlider.maxValue = 100;
        hungerSlider.value = hunger;
        thirstSlider.maxValue = 100;
        thirstSlider.value = thirst;

        interactionText = interactionTextObject.GetComponent<TMP_Text>();
        //interactionTextObject.SetActive(false);
        interactionText.text = "";

        LoadGame();
    }

    void Update()
    {
        // Input abfragen
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D oder Pfeile links/rechts
        movement.y = Input.GetAxisRaw("Vertical");   // W/S oder Pfeile hoch/runter

        // Hunger/Durst
        hunger -= hungerDecayRate * Time.deltaTime;
        thirst -= thirstDecayRate * Time.deltaTime;

        hunger = Mathf.Clamp(hunger, 0f, 100f);
        thirst = Mathf.Clamp(thirst, 0f, 100f);

        hungerSlider.value = hunger;
        thirstSlider.value = thirst;

        if (nearbyFood != null)
        {
            interactionText.text = "press F to eat";
            //interactionTextObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                EatFood(nearbyFood.nutrition);
                Destroy(nearbyFood.gameObject); // Entfernt das Essen
                nearbyFood = null;
                //interactionText.text = "";

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

        if (nearbyStone != null)
        {
            interactionText.text = "press E to pick up stone";

            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpStone();
            }
        }

        if (nearbyTree != null)
        {
            interactionText.text = "press E to pick up wood";

            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpTree();
            }
        }

        if (nearbySilver != null)
        {
            interactionText.text = "press E to pick up silver";

            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpSilver();
            }
        }

        if (nearbyGold != null)
        {
            interactionText.text = "press E to pick up gold";

            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpGold();
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItem(rightHandItem, ref rightHandItem, rightHandItemSave);
            rightHandItemSave = "";
        }

        // Drop linke Hand
        if (Input.GetKeyDown(KeyCode.Y))
        {
            DropItem(leftHandItem, ref leftHandItem, leftHandItemSave);
            leftHandItemSave = "";
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveSystem.ResetSave();
        }

        if (hunger <= 0 || thirst <= 0)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            nearbyFood = other.GetComponent<FoodScript>();
        }

        if (other.CompareTag("Water"))
        {
            nearbyWater = other.GetComponent<WaterScript>();
        }

        if (other.CompareTag("Stone") || other.CompareTag("ItemStone"))
        {
            nearbyStone = other.GetComponent<StoneScript>();
        }

        if (other.CompareTag("Tree") || other.CompareTag("ItemTree"))
        {
            nearbyTree = other.GetComponent<TreeScript>();
        }

        if (other.CompareTag("Silver") || other.CompareTag("ItemSilver"))
        {
            nearbySilver = other.GetComponent<SilverScript>();
        }

        if (other.CompareTag("Gold") || other.CompareTag("ItemGold"))
        {
            nearbyGold = other.GetComponent<GoldScript>();
        }

        if (other.CompareTag("caveEntry"))
        {
            SaveGame();
            rb.position = new Vector3(-45f, 26f, 0f);
            SceneManager.LoadScene("cave");
        }

        if (other.CompareTag("caveExit"))
        {
            SaveGame();
            rb.position = new Vector3(0f, 19.5f, 0f);
            SceneManager.LoadScene("map");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
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

        if (other.CompareTag("Stone"))
        {
            if (other.GetComponent<StoneScript>() == nearbyStone)
            {
                nearbyStone = null;
                interactionText.text = "";
            }
        }

        if (other.CompareTag("Tree"))
        {
            if (other.GetComponent<TreeScript>() == nearbyTree)
            {
                nearbyTree = null;
                interactionText.text = "";
            }
        }

        if (other.CompareTag("Silver"))
        {
            if (other.GetComponent<SilverScript>() == nearbySilver)
            {
                nearbySilver = null;
                interactionText.text = "";
            }
        }

        if (other.CompareTag("Gold"))
        {
            if (other.GetComponent<GoldScript>() == nearbyGold)
            {
                nearbyGold = null;
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

    void PickUpStone()
    {
        if (rightHandItem == null)
        {
            Debug.Log("So weit sind wir nie gekommen");
            rightHandItem = Instantiate(stonePrefab, rightHandHold.position, rightHandHold.rotation);
            rightHandItem.tag = "ItemStone";
            rightHandItem.transform.SetParent(rightHandHold);
            rightHandItem.transform.localScale = Vector3.one * 0.5f; // kleiner in der Hand
            rightHandItemSave = "Stone";
            if(nearbyStone.CompareTag("ItemStone")){
                Destroy(nearbyStone.gameObject);
            }
        }
        else if (leftHandItem == null)
        {
            
            leftHandItem = Instantiate(stonePrefab, leftHandHold.position, leftHandHold.rotation);
            leftHandItem.tag = "ItemStone";
            leftHandItem.transform.SetParent(leftHandHold);
            leftHandItem.transform.localScale = Vector3.one * 0.5f;
            leftHandItemSave = "Stone";
            if(nearbyStone.CompareTag("ItemStone")){
                Destroy(nearbyStone.gameObject);
            }
        }
        else
        {
            Debug.Log("Beide Hände voll!");
        }
    }

    void PickUpTree()
    {
        if (rightHandItem == null)
        {
            rightHandItem = Instantiate(treePrefab, rightHandHold.position, rightHandHold.rotation);
            rightHandItem.tag = "ItemTree";
            rightHandItem.transform.SetParent(rightHandHold);
            rightHandItem.transform.localScale = Vector3.one * 0.5f; // kleiner in der Hand
            rightHandItemSave = "Tree";
            if(nearbyTree.CompareTag("ItemTree")){
                Destroy(nearbyTree.gameObject);
            }
        }
        else if (leftHandItem == null)
        {
            leftHandItem = Instantiate(treePrefab, leftHandHold.position, leftHandHold.rotation);
            leftHandItem.tag = "ItemTree";
            leftHandItem.transform.SetParent(leftHandHold);
            leftHandItem.transform.localScale = Vector3.one * 0.5f;
            leftHandItemSave = "Tree";
            if(nearbyTree.CompareTag("ItemTree")){
                Destroy(nearbyTree.gameObject);
            }
        }
        else
        {
            Debug.Log("Beide Hände voll!");
        }
    }

    void PickUpSilver()
    {
        Debug.Log("Picking Silver");

        if (rightHandItem == null)
        {
            rightHandItem = Instantiate(silverPrefab, rightHandHold.position, rightHandHold.rotation);
            rightHandItem.tag = "ItemSilver";
            rightHandItem.transform.SetParent(rightHandHold);
            rightHandItem.transform.localScale = Vector3.one * 0.5f; // kleiner in der Hand
            rightHandItemSave = "Silver";
            if(nearbySilver.CompareTag("ItemSilver")){
                Destroy(nearbySilver.gameObject);
            }
        }
        else if (leftHandItem == null)
        {
            leftHandItem = Instantiate(silverPrefab, leftHandHold.position, leftHandHold.rotation);
            leftHandItem.tag = "ItemSilver";
            leftHandItem.transform.SetParent(leftHandHold);
            leftHandItem.transform.localScale = Vector3.one * 0.5f;
            leftHandItemSave = "Silver";
            if(nearbySilver.CompareTag("ItemSilver")){
                Destroy(nearbySilver.gameObject);
            }
        }
        else
        {
            Debug.Log("Beide Hände voll!");
        }
    }

    void PickUpGold()
    {
        Debug.Log("Picking Gold");


        if (rightHandItem == null)
        {
            rightHandItem = Instantiate(goldPrefab, rightHandHold.position, rightHandHold.rotation);
            rightHandItem.tag = "ItemGold";
            rightHandItem.transform.SetParent(rightHandHold);
            rightHandItem.transform.localScale = Vector3.one * 0.5f; // kleiner in der Hand
            rightHandItemSave = "Gold";
            if(nearbyGold.CompareTag("ItemGold")){
                Destroy(nearbyGold.gameObject);
            }
            
        }
        else if (leftHandItem == null)
        {
            leftHandItem = Instantiate(goldPrefab, leftHandHold.position, leftHandHold.rotation);
            leftHandItem.tag = "ItemGold";
            leftHandItem.transform.SetParent(leftHandHold);
            leftHandItem.transform.localScale = Vector3.one * 0.5f;
            leftHandItemSave = "Gold";
            if(nearbyGold.CompareTag("ItemGold")){
                Destroy(nearbyGold.gameObject);
            }
        }
        else
        {
            Debug.Log("Beide Hände voll!");
        }
    }

    void DropItem(GameObject item, ref GameObject slot, string prefabName)
    {
        if (item != null)
        {
            item.transform.SetParent(null);

            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb == null) rb = item.AddComponent<Rigidbody>(); // falls noch keiner dran ist

            Collider col = item.GetComponent<Collider>();
            if (col != null) col.enabled = true; // sicherstellen, dass es mit der Welt kollidiert

            //rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
            MarkItem(item, prefabName);

            slot = null;
        }
    }

    void RestoreItem(string itemName, bool isRightHand)
    {
        Transform handHold = isRightHand ? rightHandHold : leftHandHold;
        ref GameObject handItem = ref (isRightHand ? ref rightHandItem : ref leftHandItem);
        ref string handSave = ref (isRightHand ? ref rightHandItemSave : ref leftHandItemSave);

        GameObject prefab = null;

        switch (itemName)
        {
            case "Stone": prefab = stonePrefab; break;
            case "Tree":  prefab = treePrefab; break;
            case "Silver": prefab = silverPrefab; break;
            case "Gold": prefab = goldPrefab; break;
            default: return; // kein Item
        }

        if (prefab != null)
        {
            handItem = Instantiate(prefab, handHold.position, handHold.rotation);
            handItem.tag = "Item" + itemName;
            handItem.transform.SetParent(handHold);
            handItem.transform.localScale = Vector3.one * 0.5f;
            handSave = itemName;
        }
    }

    void MarkItem(GameObject item, string prefabName)
    {
        var marker = item.AddComponent<WorldItemMarker>();
        marker.prefabName = prefabName;
    }



    /*public void SaveGame()
    {
        PlayerData data = new PlayerData();
        data.hunger = hunger;
        data.thirst = thirst;
        data.rightHandItemSave = rightHandItemSave;
        data.leftHandItemSave = leftHandItemSave;
        //data.position = new float[] { transform.position.x, transform.position.y, transform.position.z };
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
            hunger = data.hunger;
            thirst = data.thirst;

            RestoreItem(data.rightHandItemSave, true);  // rechte Hand
            RestoreItem(data.leftHandItemSave, false);  // linke Hand

            foreach (WorldItemData itemData in data.worldItems)
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
                    GameObject obj = Instantiate(prefab,
                        new Vector3(itemData.posX, itemData.posY, itemData.posZ),
                        Quaternion.Euler(itemData.rotX, itemData.rotY, itemData.rotZ));

                    MarkItem(obj, itemData.prefabName); 
                }
            }

            //transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        }
    }*/


    void Die()
    {
        Debug.Log("Spieler ist gestorben!");
    }
}
