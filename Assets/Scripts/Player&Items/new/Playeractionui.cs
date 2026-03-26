using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerActionUI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private PlayerCrafter   _crafter;

    [Header("Pickup Button 1")]
    [SerializeField] private GameObject  _pickupButton1;
    [SerializeField] private TMP_Text    _pickupButton1Label;

    [Header("Pickup Button 2")]
    [SerializeField] private GameObject  _pickupButton2;
    [SerializeField] private TMP_Text    _pickupButton2Label;

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
        // Hide all buttons initially
        SetPickupButton(_pickupButton1, _pickupButton1Label, null);
        SetPickupButton(_pickupButton2, _pickupButton2Label, null);
        _craftButton.SetActive(false);
    }

    // ── Called by UI Button OnClick events (assign in Inspector) ─────────────

    public void OnPickupButton1Clicked()
    {
        var items = _inventory.NearbyItems;
        if (items.Count >= 1)
            _inventory.TryPickUp(items[0]);
    }

    public void OnPickupButton2Clicked()
    {
        var items = _inventory.NearbyItems;
        if (items.Count >= 2)
            _inventory.TryPickUp(items[1]);
    }

    public void OnCraftButtonClicked()
    {
        _crafter.TryCraft();
    }

    // ── Private ───────────────────────────────────────────────────────────────

    /// <summary>
    /// Called whenever inventory state changes — updates all button visibility.
    /// </summary>
    private void Refresh()
    {
        var items = _inventory.NearbyItems;

        // Pickup button 1
        SetPickupButton(
            _pickupButton1,
            _pickupButton1Label,
            items.Count >= 1 ? items[0] : null);

        // Pickup button 2
        SetPickupButton(
            _pickupButton2,
            _pickupButton2Label,
            items.Count >= 2 ? items[1] : null);

        // Craft button — only visible if current hand items form a valid recipe
        bool showCraft = _crafter.CanCraft();
        _craftButton.SetActive(showCraft);

        if (showCraft && _craftButtonLabel != null)
            _craftButtonLabel.text = "Craft";
    }

    private void SetPickupButton(GameObject button, TMP_Text label, Item item)
    {
        bool show = item != null;
        button.SetActive(show);

        if (show && label != null)
            label.text = item.ItemName;
    }
}