using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Hand Transforms")]
    public Transform rightHandHold;
    public Transform leftHandHold;

    // ── Events ───────────────────────────────────────────────────────────────
    public event Action<ItemData> OnItemPickedUp;
    public event Action<ItemData> OnItemDropped;
    public event Action           OnHandsChanged; // fired whenever hand contents change

    // ── Public read-only state ────────────────────────────────────────────────
    public string RightHandItemName { get; private set; } = string.Empty;
    public string LeftHandItemName  { get; private set; } = string.Empty;
    public bool   BothHandsFull     => _rightHandItem != null && _leftHandItem != null;

    // Up to 2 nearby items exposed for the UI
    public IReadOnlyList<Item> NearbyItems => _nearbyItems;

    // ── Private state ─────────────────────────────────────────────────────────
    private GameObject    _rightHandItem;
    private GameObject    _leftHandItem;
    private List<Item>    _nearbyItems = new List<Item>(2);

    // ── Unity lifecycle ───────────────────────────────────────────────────────

    private void OnTriggerEnter2D(Collider2D other)
    {
        var item = other.GetComponent<Item>();
        if (item == null || !item.AbleToPickup) return;
        if (_nearbyItems.Count >= 2) return;
        if (_nearbyItems.Contains(item)) return;

        _nearbyItems.Add(item);
        OnHandsChanged?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var item = other.GetComponent<Item>();
        if (item == null) return;

        if (_nearbyItems.Remove(item))
            OnHandsChanged?.Invoke();
    }

    // ── Public API — called by PlayerActionUI buttons ─────────────────────────

    public void TryPickUp(Item item)
    {
        if (item == null || !item.AbleToPickup) return;

        if (_rightHandItem == null)
            SpawnIntoSlot(item.data, ref _rightHandItem, rightHandHold, isRight: true);
        else if (_leftHandItem == null)
            SpawnIntoSlot(item.data, ref _leftHandItem, leftHandHold, isRight: false);
        else
            return; // both hands full

        if (item.DestroyOnPickup)
            Destroy(item.gameObject);
        else
            Debug.Log($"[PlayerInventory] {item.ItemName} entnommen, Quelle bleibt bestehen.");

        _nearbyItems.Remove(item);
        OnHandsChanged?.Invoke();
    }

    /// <summary>Drops the item in the right hand.</summary>
    public void DropRight() => DropSlot(ref _rightHandItem, isRight: true);

    /// <summary>Drops the item in the left hand.</summary>
    public void DropLeft()  => DropSlot(ref _leftHandItem, isRight: false);

    /// <summary>
    /// Spawns a crafting result into the first free hand.
    /// Called by PlayerCrafter after a successful craft.
    /// </summary>
    public bool TrySpawnIntoHand(ItemData itemData)
    {
        if (itemData?.prefab == null)
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

        return false;
    }

    /// <summary>Destroys both hand items. Called by PlayerCrafter to consume ingredients.</summary>
    public void ClearBothHands()
    {
        DestroyHandItem(ref _rightHandItem, isRight: true);
        DestroyHandItem(ref _leftHandItem,  isRight: false);
        OnHandsChanged?.Invoke();
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private void SpawnIntoSlot(ItemData itemData, ref GameObject slot, Transform hold, bool isRight)
    {
        slot = Instantiate(itemData.prefab, hold.position, hold.rotation);
        slot.transform.SetParent(hold);

        if (isRight) RightHandItemName = itemData.itemName;
        else         LeftHandItemName  = itemData.itemName;

        OnItemPickedUp?.Invoke(itemData);
        OnHandsChanged?.Invoke();
    }

    private void DropSlot(ref GameObject slot, bool isRight)
    {
        if (slot == null) return;

        slot.transform.SetParent(null);

        if (slot.GetComponent<Rigidbody2D>() == null)
            slot.AddComponent<Rigidbody2D>();

        var col = slot.GetComponent<Collider2D>();
        if (col != null) col.enabled = true;

        var droppedData = slot.GetComponent<Item>()?.data;
        if (droppedData != null) OnItemDropped?.Invoke(droppedData);

        slot = null;

        if (isRight) RightHandItemName = string.Empty;
        else         LeftHandItemName  = string.Empty;

        OnHandsChanged?.Invoke();
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