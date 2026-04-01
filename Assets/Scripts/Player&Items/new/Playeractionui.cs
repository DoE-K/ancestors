using UnityEngine;
using TMPro;

public class PlayerActionUI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private PlayerCrafter   _crafter;

    [Header("Hand Button 1 (Right Hand)")]
    [SerializeField] private GameObject _handButton1;
    [SerializeField] private TMP_Text   _handButton1Label;

    [Header("Hand Button 2 (Left Hand)")]
    [SerializeField] private GameObject _handButton2;
    [SerializeField] private TMP_Text   _handButton2Label;

    [Header("Craft Button")]
    [SerializeField] private GameObject _craftButton;
    [SerializeField] private TMP_Text   _craftButtonLabel;

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

        var item = GetNearbyItemForSlot(rightHandOccupied: false, leftHandOccupied:
            !string.IsNullOrEmpty(_inventory.LeftHandItemName), slotIndex: 0);

        if (item != null) _inventory.TryPickUp(item);
    }

    public void OnHandButton2Clicked()
    {
        if (!string.IsNullOrEmpty(_inventory.LeftHandItemName))
        {
            _inventory.DropLeft();
            return;
        }

        bool rightOccupied = !string.IsNullOrEmpty(_inventory.RightHandItemName);

        int slotIndex = rightOccupied ? 0 : 1;

        var item = GetNearbyItemForSlot(
            rightHandOccupied: rightOccupied,
            leftHandOccupied: false,
            slotIndex: slotIndex);

        if (item != null) _inventory.TryPickUp(item);
    }

    public void OnCraftButtonClicked() => _crafter.TryCraft();

    // ── Refresh ───────────────────────────────────────────────────────────────

    private void Refresh()
    {
        var nearby         = _inventory.NearbyItems;
        bool rightOccupied = !string.IsNullOrEmpty(_inventory.RightHandItemName);
        bool leftOccupied  = !string.IsNullOrEmpty(_inventory.LeftHandItemName);

        if (rightOccupied)
        {
            Show(_handButton1, _handButton1Label, $"Drop {_inventory.RightHandItemName}");
        }
        else if (nearby.Count > 0)
        {
            // nearby[0] is always claimed by button 1 when right hand is empty
            Show(_handButton1, _handButton1Label, $"Pickup {nearby[0].ItemName}");
        }
        else
        {
            _handButton1.SetActive(false);
        }

        if (leftOccupied)
        {
            Show(_handButton2, _handButton2Label, $"Drop {_inventory.LeftHandItemName}");
        }
        else
        {
            // If right hand is empty it already claims nearby[0], so left gets nearby[1]
            // If right hand is occupied it claims nothing, so left gets nearby[0]
            int nearbyIndexForLeft = rightOccupied ? 0 : 1;

            if (nearby.Count > nearbyIndexForLeft)
                Show(_handButton2, _handButton2Label,
                    $"Pickup {nearby[nearbyIndexForLeft].ItemName}");
            else
                _handButton2.SetActive(false);
        }

        // Craft button
        bool showCraft = _crafter.CanCraft();
        _craftButton.SetActive(showCraft);
        if (showCraft && _craftButtonLabel != null)
            _craftButtonLabel.text = "Craft";
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void Show(GameObject button, TMP_Text label, string text)
    {
        button.SetActive(true);
        label.text = text;
    }

    private Item GetNearbyItemForSlot(bool rightHandOccupied, bool leftHandOccupied, int slotIndex)
    {
        var nearby = _inventory.NearbyItems;
        if (nearby.Count > slotIndex) return nearby[slotIndex];
        return null;
    }
}