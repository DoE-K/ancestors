using UnityEngine;
using TMPro;

public class PlayerActionUI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private PlayerCrafter   _crafter;

    [Header("Hand Button — Left (or first hand)")]
    [SerializeField] private GameObject  _handButton1;
    [SerializeField] private TMP_Text    _handButton1Label;

    [Header("Hand Button — Right (or second hand)")]
    [SerializeField] private GameObject  _handButton2;
    [SerializeField] private TMP_Text    _handButton2Label;

    [Header("Craft Button")]
    [SerializeField] private GameObject  _craftButton;
    [SerializeField] private TMP_Text    _craftButtonLabel;

    // ── Unity lifecycle ───────────────────────────────────────────────────────

    private void OnEnable()
    {
        _inventory.OnHandsChanged += Refresh;
    }

    private void OnDisable()
    {
        _inventory.OnHandsChanged -= Refresh;
    }

    private void Start()
    {
        _handButton1.SetActive(false);
        _handButton2.SetActive(false);
        _craftButton.SetActive(false);
    }

    // ── Button callbacks (assign in Inspector OnClick) ────────────────────────

    public void OnHandButton1Clicked()
    {
        // If right hand is occupied → drop it
        if (!string.IsNullOrEmpty(_inventory.RightHandItemName))
        {
            _inventory.DropRight();
            return;
        }

        // Otherwise pick up the first nearby item
        if (_inventory.NearbyItems.Count >= 1)
            _inventory.TryPickUp(_inventory.NearbyItems[0]);
    }

    public void OnHandButton2Clicked()
    {
        // If left hand is occupied → drop it
        if (!string.IsNullOrEmpty(_inventory.LeftHandItemName))
        {
            _inventory.DropLeft();
            return;
        }

        // Otherwise pick up the second nearby item
        if (_inventory.NearbyItems.Count >= 2)
            _inventory.TryPickUp(_inventory.NearbyItems[1]);
        else if (_inventory.NearbyItems.Count >= 1)
            _inventory.TryPickUp(_inventory.NearbyItems[0]);
    }

    public void OnCraftButtonClicked()
    {
        _crafter.TryCraft();
    }

    // ── Refresh ───────────────────────────────────────────────────────────────

    private void Refresh()
    {
        RefreshHandButton(
            button:    _handButton1,
            label:     _handButton1Label,
            heldItem:  _inventory.RightHandItemName,
            nearbyIndex: 0);

        RefreshHandButton(
            button:    _handButton2,
            label:     _handButton2Label,
            heldItem:  _inventory.LeftHandItemName,
            nearbyIndex: 1);

        bool showCraft = _crafter.CanCraft();
        _craftButton.SetActive(showCraft);
        if (showCraft && _craftButtonLabel != null)
            _craftButtonLabel.text = "Craft";
    }

    /// <summary>
    /// Updates a single hand button:
    ///   - Hand occupied → "Drop [name]"
    ///   - Hand empty + nearby item available → "Pickup [name]"
    ///   - Neither → hide
    /// </summary>
    private void RefreshHandButton(
        GameObject button,
        TMP_Text   label,
        string     heldItem,
        int        nearbyIndex)
    {
        bool handOccupied = !string.IsNullOrEmpty(heldItem);

        if (handOccupied)
        {
            button.SetActive(true);
            label.text = $"Drop {heldItem}";
            return;
        }

        // Hand is empty — show pickup if a nearby item exists for this slot
        var nearby = _inventory.NearbyItems;
        if (nearby.Count > nearbyIndex)
        {
            button.SetActive(true);
            label.text = $"Pickup {nearby[nearbyIndex].ItemName}";
            return;
        }

        // Nothing to show
        button.SetActive(false);
    }
}