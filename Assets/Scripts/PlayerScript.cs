using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

        
        if (nearbyFood != null)
        {
            interactionText.text = "press F to eat";
            if (Input.GetKeyDown(KeyCode.F))
            {
                EatFood(nearbyFood.nutrition);
                Destroy(nearbyFood.gameObject);
                nearbyFood = null;
            }
        }
        else if (nearbyWater != null)
        {
            interactionText.text = "press R to drink";
            if (Input.GetKeyDown(KeyCode.R))
            {
                DrinkWater(nearbyWater.thirst);
            }
        }
        else if (nearbyStone != null)
        {
            interactionText.text = "press E to pick up stone";
            if (Input.GetKeyDown(KeyCode.E)) PickUpStone();
        }
        else if (nearbyTree != null)
        {
            interactionText.text = "press E to pick up wood";
            if (Input.GetKeyDown(KeyCode.E)) PickUpTree();
        }
        else if (nearbySilver != null)
        {
            interactionText.text = "press E to pick up silver";
            if (Input.GetKeyDown(KeyCode.E)) PickUpSilver();
        }
        else if (nearbyGold != null)
        {
            interactionText.text = "press E to pick up gold";
            if (Input.GetKeyDown(KeyCode.E)) PickUpGold();
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
        if (Input.GetKeyDown(KeyCode.Y))
        {
            DropItem(leftHandItem, ref leftHandItem, leftHandItemSave);
            leftHandItemSave = "";
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
        if (other.CompareTag("Food")) nearbyFood = other.GetComponent<FoodScript>();
        if (other.CompareTag("Water")) nearbyWater = other.GetComponent<WaterScript>();
        if (other.CompareTag("Stone") || other.CompareTag("ItemStone")) nearbyStone = other.GetComponent<StoneScript>();
        if (other.CompareTag("Tree") || other.CompareTag("ItemTree")) nearbyTree = other.GetComponent<TreeScript>();
        if (other.CompareTag("Silver") || other.CompareTag("ItemSilver")) nearbySilver = other.GetComponent<SilverScript>();
        if (other.CompareTag("Gold") || other.CompareTag("ItemGold")) nearbyGold = other.GetComponent<GoldScript>();

        if (other.CompareTag("caveEntry"))
        {
            FindObjectOfType<GameDataManager>().SaveGame();
            rb.position = new Vector3(-45f, 26f, 0f);
            SceneManager.LoadScene("cave");
        }
        if (other.CompareTag("caveExit"))
        {
            FindObjectOfType<GameDataManager>().SaveGame();
            rb.position = new Vector3(0f, 19.5f, 0f);
            SceneManager.LoadScene("map");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<FoodScript>() == nearbyFood) nearbyFood = null;
        if (other.GetComponent<WaterScript>() == nearbyWater) nearbyWater = null;
        if (other.GetComponent<StoneScript>() == nearbyStone) nearbyStone = null;
        if (other.GetComponent<TreeScript>() == nearbyTree) nearbyTree = null;
        if (other.GetComponent<SilverScript>() == nearbySilver) nearbySilver = null;
        if (other.GetComponent<GoldScript>() == nearbyGold) nearbyGold = null;
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
            rightHandItem = Instantiate(stonePrefab, rightHandHold.position, rightHandHold.rotation);
            rightHandItem.tag = "ItemStone";
            rightHandItem.transform.SetParent(rightHandHold);
            rightHandItem.transform.localScale = Vector3.one * 0.5f;
            rightHandItemSave = "Stone";
            if (nearbyStone.CompareTag("ItemStone")) Destroy(nearbyStone.gameObject);
        }
        else if (leftHandItem == null)
        {
            leftHandItem = Instantiate(stonePrefab, leftHandHold.position, leftHandHold.rotation);
            leftHandItem.tag = "ItemStone";
            leftHandItem.transform.SetParent(leftHandHold);
            leftHandItem.transform.localScale = Vector3.one * 0.5f;
            leftHandItemSave = "Stone";
            if (nearbyStone.CompareTag("ItemStone")) Destroy(nearbyStone.gameObject);
        }
    }

    void PickUpTree()
    {
        if (rightHandItem == null)
        {
            rightHandItem = Instantiate(treePrefab, rightHandHold.position, rightHandHold.rotation);
            rightHandItem.tag = "ItemTree";
            rightHandItem.transform.SetParent(rightHandHold);
            rightHandItem.transform.localScale = Vector3.one * 0.5f;
            rightHandItemSave = "Tree";
            if (nearbyTree.CompareTag("ItemTree")) Destroy(nearbyTree.gameObject);
        }
        else if (leftHandItem == null)
        {
            leftHandItem = Instantiate(treePrefab, leftHandHold.position, leftHandHold.rotation);
            leftHandItem.tag = "ItemTree";
            leftHandItem.transform.SetParent(leftHandHold);
            leftHandItem.transform.localScale = Vector3.one * 0.5f;
            leftHandItemSave = "Tree";
            if (nearbyTree.CompareTag("ItemTree")) Destroy(nearbyTree.gameObject);
        }
    }

    void PickUpSilver()
    {
        if (rightHandItem == null)
        {
            rightHandItem = Instantiate(silverPrefab, rightHandHold.position, rightHandHold.rotation);
            rightHandItem.tag = "ItemSilver";
            rightHandItem.transform.SetParent(rightHandHold);
            rightHandItem.transform.localScale = Vector3.one * 0.5f;
            rightHandItemSave = "Silver";
            if (nearbySilver.CompareTag("ItemSilver")) Destroy(nearbySilver.gameObject);
        }
        else if (leftHandItem == null)
        {
            leftHandItem = Instantiate(silverPrefab, leftHandHold.position, leftHandHold.rotation);
            leftHandItem.tag = "ItemSilver";
            leftHandItem.transform.SetParent(leftHandHold);
            leftHandItem.transform.localScale = Vector3.one * 0.5f;
            leftHandItemSave = "Silver";
            if (nearbySilver.CompareTag("ItemSilver")) Destroy(nearbySilver.gameObject);
        }
    }

    void PickUpGold()
    {
        if (rightHandItem == null)
        {
            rightHandItem = Instantiate(goldPrefab, rightHandHold.position, rightHandHold.rotation);
            rightHandItem.tag = "ItemGold";
            rightHandItem.transform.SetParent(rightHandHold);
            rightHandItem.transform.localScale = Vector3.one * 0.5f;
            rightHandItemSave = "Gold";
            if (nearbyGold.CompareTag("ItemGold")) Destroy(nearbyGold.gameObject);
        }
        else if (leftHandItem == null)
        {
            leftHandItem = Instantiate(goldPrefab, leftHandHold.position, leftHandHold.rotation);
            leftHandItem.tag = "ItemGold";
            leftHandItem.transform.SetParent(leftHandHold);
            leftHandItem.transform.localScale = Vector3.one * 0.5f;
            leftHandItemSave = "Gold";
            if (nearbyGold.CompareTag("ItemGold")) Destroy(nearbyGold.gameObject);
        }
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

            MarkItem(item, prefabName);
            slot = null;
        }
    }

    void RestoreItem(string itemName, bool isRightHand)
    {
        Transform handHold = isRightHand ? rightHandHold : leftHandHold;
        GameObject prefab = null;

        switch (itemName)
        {
            case "Stone": prefab = stonePrefab; break;
            case "Tree": prefab = treePrefab; break;
            case "Silver": prefab = silverPrefab; break;
            case "Gold": prefab = goldPrefab; break;
            default: return;
        }

        if (prefab != null)
        {
            var handItem = Instantiate(prefab, handHold.position, handHold.rotation);
            handItem.tag = "Item" + itemName;
            handItem.transform.SetParent(handHold);
            handItem.transform.localScale = Vector3.one * 0.5f;

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
