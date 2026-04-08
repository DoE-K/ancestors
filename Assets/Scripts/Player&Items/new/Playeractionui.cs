using UnityEngine;
using UnityEngine.UI;

public class PlayerActionUI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private PlayerCrafter   _crafter;
    [SerializeField] private ItemRegistry    _itemRegistry;

    [Header("Hand Button 1 (Right Hand)")]
    [SerializeField] private GameObject _handButton1;
    [SerializeField] private Image      _handButton1Icon;

    [Header("Hand Button 2 (Left Hand)")]
    [SerializeField] private GameObject _handButton2;
    [SerializeField] private Image      _handButton2Icon;

    [Header("Craft Button")]
    [SerializeField] private GameObject _craftButton;
    [SerializeField] private Image      _craftButtonIcon;

    [Header("Default Icons")]
    [Tooltip("Icon shown when the hand is empty and an item is nearby (pickup action).")]
    [SerializeField] private Sprite _pickupIcon;
    [Tooltip("Icon shown on the craft button.")]
    [SerializeField] private Sprite _craftIcon;

    // ── Unity lifecycle ───────────────────────────────────────────────────────

    private void OnEnable()  => _inventory.OnHandsChanged += Refresh;
    private void OnDisable() => _inventory.OnHandsChanged -= Refresh;

    private void Start()
    {
        _handButton1.SetActive(false);
        _handButton2.SetActive(false);
        _craftButton.SetActive(false);
    }

    // ── Button callbacks ──────────────────────────────────────────────────────

    public void OnHandButton1Clicked()
    {
        if (!string.IsNullOrEmpty(_inventory.RightHandItemName))
        {
            _inventory.DropRight();
            return;
        }

        if (_inventory.NearbyItems.Count >= 1)
            _inventory.TryPickUp(_inventory.NearbyItems[0]);
    }

    public void OnHandButton2Clicked()
    {
        if (!string.IsNullOrEmpty(_inventory.LeftHandItemName))
        {
            _inventory.DropLeft();
            return;
        }

        bool rightOccupied = !string.IsNullOrEmpty(_inventory.RightHandItemName);
        int  nearbyIndex   = rightOccupied ? 0 : 1;
        var  nearby        = _inventory.NearbyItems;

        if (nearby.Count > nearbyIndex)
            _inventory.TryPickUp(nearby[nearbyIndex]);
        else if (nearby.Count > 0)
            _inventory.TryPickUp(nearby[0]);
    }

    public void OnCraftButtonClicked() => _crafter.TryCraft();

    // ── Refresh ───────────────────────────────────────────────────────────────

    private void Refresh()
    {
        var  nearby        = _inventory.NearbyItems;
        bool rightOccupied = !string.IsNullOrEmpty(_inventory.RightHandItemName);
        bool leftOccupied  = !string.IsNullOrEmpty(_inventory.LeftHandItemName);

        // Button 1 — Right hand
        if (rightOccupied)
        {
            // Show the held item's sprite
            Show(_handButton1, _handButton1Icon,
                GetItemSprite(_inventory.RightHandItemName));
        }
        else if (nearby.Count > 0)
        {
            // Show pickup icon
            Show(_handButton1, _handButton1Icon, _pickupIcon);
        }
        else
        {
            _handButton1.SetActive(false);
        }

        // Button 2 — Left hand
        if (leftOccupied)
        {
            Show(_handButton2, _handButton2Icon,
                GetItemSprite(_inventory.LeftHandItemName));
        }
        else
        {
            int nearbyIndexForLeft = rightOccupied ? 0 : 1;
            if (nearby.Count > nearbyIndexForLeft)
                Show(_handButton2, _handButton2Icon, _pickupIcon);
            else
                _handButton2.SetActive(false);
        }

        // Craft button
        bool showCraft = _crafter.CanCraft();
        _craftButton.SetActive(showCraft);
        if (showCraft)
            _craftButtonIcon.sprite = _craftIcon;
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void Show(GameObject button, Image icon, Sprite sprite)
    {
        button.SetActive(true);
        icon.sprite = sprite;
    }

    /// <summary>
    /// Looks up the sprite for a held item by name via ItemRegistry.
    /// Falls back to the pickup icon if no sprite is set on the ItemData.
    /// </summary>
    private Sprite GetItemSprite(string itemName)
    {
        if (_itemRegistry != null && _itemRegistry.TryGet(itemName, out var data))
            if (data.sprite != null)
                return data.sprite;

        return _pickupIcon; // fallback
    }
}