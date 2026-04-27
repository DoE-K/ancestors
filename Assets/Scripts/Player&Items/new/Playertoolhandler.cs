using UnityEngine;
using TMPro;

/// <summary>
/// Detects when the player holds a tool and handles activation.
///
/// Activation: the UI "Use Tool" button calls OnUseToolClicked()
///   OR the player presses KeyCode.F (configurable).
///
/// Also calls WhileHeld() every frame for continuous effects (Telescope etc.)
/// and OnUnequip() when the hand changes.
///
/// Add this component to the Player GameObject.
/// </summary>
public class PlayerToolHandler : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private CameraZoom      _cameraZoom;
    [SerializeField] private DayNight        _dayNight;

    [Header("Input (optional keyboard fallback)")]
    [SerializeField] private KeyCode _useKey = KeyCode.F;

    [Header("UI (optional)")]
    [Tooltip("Button shown when a tool is in the active hand.")]
    [SerializeField] private GameObject  _useToolButton;
    [SerializeField] private TMP_Text    _useToolHint;

    // ── Private state ─────────────────────────────────────────────────────────
    private PlayerContext _ctx;
    private ItemEffect    _lastRightEffect;
    private ItemEffect    _lastLeftEffect;

    // ── Unity lifecycle ───────────────────────────────────────────────────────

    private void Awake()
    {
        _ctx = new PlayerContext
        {
            Transform = transform,
            Inventory = _inventory,
            Camera    = _cameraZoom,
            DayNight  = _dayNight
        };
    }

    private void OnEnable()  => _inventory.OnHandsChanged += OnHandsChanged;
    private void OnDisable() => _inventory.OnHandsChanged -= OnHandsChanged;

    private void Update()
    {
        // Continuous effect while held
        GetActiveEffect(out var effect, out _);
        effect?.WhileHeld(_ctx);

        // Keyboard fallback
        if (Input.GetKeyDown(_useKey))
            UseTool();

        UpdateToolButton(effect);
    }

    // ── Public API — called by UI button ─────────────────────────────────────

    public void OnUseToolClicked() => UseTool();

    // ── Private methods ───────────────────────────────────────────────────────

    private void UseTool()
    {
        if (!GetActiveEffect(out var effect, out _)) return;
        effect.Use(_ctx);
    }

    private void OnHandsChanged()
    {
        // Call OnUnequip on effects that are no longer held
        var rightEffect = GetEffectForItem(_inventory.RightHandItemName);
        var leftEffect  = GetEffectForItem(_inventory.LeftHandItemName);

        if (_lastRightEffect != null && _lastRightEffect != rightEffect)
            _lastRightEffect.OnUnequip(_ctx);

        if (_lastLeftEffect != null && _lastLeftEffect != leftEffect)
            _lastLeftEffect.OnUnequip(_ctx);

        _lastRightEffect = rightEffect;
        _lastLeftEffect  = leftEffect;
    }

    /// <summary>
    /// Returns the first active tool effect found in either hand.
    /// Right hand takes priority.
    /// </summary>
    private bool GetActiveEffect(out ItemEffect effect, out bool isRight)
    {
        effect  = GetEffectForItem(_inventory.RightHandItemName);
        isRight = true;

        if (effect != null) return true;

        effect  = GetEffectForItem(_inventory.LeftHandItemName);
        isRight = false;

        return effect != null;
    }

    private ItemEffect GetEffectForItem(string itemName)
    {
        if (string.IsNullOrEmpty(itemName)) return null;

        // We need ItemRegistry to look up ItemData by name
        var registry = FindAnyObjectByType<ItemRegistryHolder>();
        return registry?.Registry?.TryGet(itemName, out var data) == true
            ? data.effect
            : null;
    }

    private void UpdateToolButton(ItemEffect effect)
    {
        if (_useToolButton == null) return;
        bool hasTool = effect != null;
        _useToolButton.SetActive(hasTool);
        if (hasTool && _useToolHint != null)
            _useToolHint.text = effect.actionHint;
    }
}