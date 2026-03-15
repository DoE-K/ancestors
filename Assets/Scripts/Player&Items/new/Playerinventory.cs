using System;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [Header("Hand Transforms")]
    public Transform rightHandHold;
    public Transform leftHandHold;

    [Header("UI")]
    public TMP_Text interactionText;

    // ── Events ──────────────────────────────────────────────────────────────
    /// <summary>Fired when an item lands in any hand slot. Passes the ItemData.</summary>
    public event Action<ItemData> OnItemPickedUp;

    /// <summary>Fired when an item is dropped from any hand slot.</summary>
    public event Action<ItemData> OnItemDropped;

    // ── Public read-only state ───────────────────────────────────────────────
    public string RightHandItemName { get; private set; } = string.Empty;
    public string LeftHandItemName  { get; private set; } = string.Empty;

    // ── Private state ────────────────────────────────────────────────────────
    private GameObject _rightHandItem;
    private GameObject _leftHandItem;
    private Item _nearbyItem;

    // ── Unity lifecycle ──────────────────────────────────────────────────────

    void Update()
    {
        UpdateInteractionText();
        HandlePickupInput();
        HandleDropInput();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var item = other.GetComponent<Item>();
        if (item != null)
            _nearbyItem = item;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var item = other.GetComponent<Item>();
        if (item != null && item == _nearbyItem)
            _nearbyItem = null;
    }

    // ── Input handling ───────────────────────────────────────────────────────

    private void UpdateInteractionText()
    {
        interactionText.text = (_nearbyItem != null && _nearbyItem.AbleToPickup)
            ? _nearbyItem.ItemName
            : string.Empty;
    }

    private void HandlePickupInput()
    {
        if (_nearbyItem == null) return;
        if (!Input.GetKeyDown(KeyCode.E)) return;

        TryPickUp(_nearbyItem);
        _nearbyItem = null;
    }

    private void HandleDropInput()
    {
        if (Input.GetKeyDown(KeyCode.G))
            DropSlot(ref _rightHandItem, isRight: true);

        if (Input.GetKeyDown(KeyCode.U))
            DropSlot(ref _leftHandItem, isRight: false);
    }

    // ── Public API ───────────────────────────────────────────────────────────

    public bool TrySpawnIntoHand(ItemData itemData)
    {
        if (itemData == null || itemData.prefab == null)
        {
            Debug.LogWarning("[PlayerInventory] TrySpawnIntoHand: itemData or prefab is null.");
            return false;
        }

        if (_rightHandItem == null)
        {
            SpawnIntoSlot(itemData, ref _rightHandItem, rightHandHold, isRight: true);
            return true;
        }
        if (_leftHandItem == null)
        {
            SpawnIntoSlot(itemData, ref _leftHandItem, leftHandHold, isRight: false);
            return true;
        }

        Debug.Log("[PlayerInventory] Both hands are full.");
        return false;
    }

    public bool BothHandsFull => _rightHandItem != null && _leftHandItem != null;

    public void ClearBothHands()
    {
        DestroyHandItem(ref _rightHandItem, isRight: true);
        DestroyHandItem(ref _leftHandItem, isRight: false);
    }

    // ── Private helpers ──────────────────────────────────────────────────────

    private void TryPickUp(Item item)
    {
        if (!item.AbleToPickup) return;

        if (_rightHandItem == null)
        {
            SpawnIntoSlot(item.data, ref _rightHandItem, rightHandHold, isRight: true);
        }
        else if (_leftHandItem == null)
        {
            SpawnIntoSlot(item.data, ref _leftHandItem, leftHandHold, isRight: false);
        }
        else
        {
            return; // Both hands full — do not destroy the world item
        }

        if (item.DestroyOnPickup)
            Destroy(item.gameObject);
        else
            Debug.Log($"[PlayerInventory] {item.ItemName} entnommen, Quelle bleibt bestehen.");
    }

    private void SpawnIntoSlot(ItemData itemData, ref GameObject slot, Transform hold, bool isRight)
    {
        slot = Instantiate(itemData.prefab, hold.position, hold.rotation);
        slot.transform.SetParent(hold);

        if (isRight) RightHandItemName = itemData.itemName;
        else         LeftHandItemName  = itemData.itemName;

        OnItemPickedUp?.Invoke(itemData);
    }

    private void DropSlot(ref GameObject slot, bool isRight)
    {
        if (slot == null) return;

        slot.transform.SetParent(null);

        //if (slot.GetComponent<Rigidbody2D>() == null)
        //    slot.AddComponent<Rigidbody2D>();

        var col = slot.GetComponent<Collider2D>();
        if (col != null) col.enabled = true;

        var droppedData = slot.GetComponent<Item>()?.data;
        if (droppedData != null) OnItemDropped?.Invoke(droppedData);

        slot = null;

        if (isRight) RightHandItemName = string.Empty;
        else         LeftHandItemName  = string.Empty;
    }

    private void DestroyHandItem(ref GameObject slot, bool isRight)
    {
        if (slot == null) return;
        Destroy(slot);
        slot = null;
        if (isRight) RightHandItemName = string.Empty;
        else         LeftHandItemName  = string.Empty;
    }
}