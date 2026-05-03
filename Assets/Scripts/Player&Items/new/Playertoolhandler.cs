using UnityEngine;
using TMPro;

/// <summary>
/// Detects when the player holds a tool and handles activation.
/// Button visibility conditions:
///   FishEffect → only show when near water
///   MineEffect → only show when near cave wall
///   Other tools → always show
/// </summary>
public class PlayerToolHandler : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerInventory     _inventory;
    [SerializeField] private CameraZoom          _cameraZoom;
    [SerializeField] private DayNight            _dayNight;
    [SerializeField] private PlayerWaterDetector _waterDetector;
    [SerializeField] private PlayerWallDetector  _wallDetector;

    [Header("Input")]
    [SerializeField] private KeyCode _useKey = KeyCode.F;

    [Header("UI")]
    [SerializeField] private GameObject _useToolButton;
    [SerializeField] private TMP_Text   _useToolHint;

    private PlayerContext _ctx;
    private ItemEffect    _lastRightEffect;
    private ItemEffect    _lastLeftEffect;

    private void Awake()
    {
        _ctx = new PlayerContext
        {
            Transform     = transform,
            Inventory     = _inventory,
            Camera        = _cameraZoom,
            DayNight      = _dayNight,
            WaterDetector = _waterDetector,
            WallDetector  = _wallDetector
        };
    }

    private void OnEnable()  => _inventory.OnHandsChanged += OnHandsChanged;
    private void OnDisable() => _inventory.OnHandsChanged -= OnHandsChanged;

    private void Update()
    {
        GetActiveEffect(out var effect, out _);
        effect?.WhileHeld(_ctx);

        if (Input.GetKeyDown(_useKey))
            UseTool();

        UpdateToolButton(effect);
    }

    public void OnUseToolClicked() => UseTool();

    private void UseTool()
    {
        if (!GetActiveEffect(out var effect, out _)) return;
        effect.Use(_ctx);
    }

    private void OnHandsChanged()
    {
        var rightEffect = GetEffectForItem(_inventory.RightHandItemName);
        var leftEffect  = GetEffectForItem(_inventory.LeftHandItemName);

        if (_lastRightEffect != null && _lastRightEffect != rightEffect)
            _lastRightEffect.OnUnequip(_ctx);
        if (_lastLeftEffect != null && _lastLeftEffect != leftEffect)
            _lastLeftEffect.OnUnequip(_ctx);

        _lastRightEffect = rightEffect;
        _lastLeftEffect  = leftEffect;
    }

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
        var holder = FindAnyObjectByType<ItemRegistryHolder>();
        return holder?.Registry?.TryGet(itemName, out var data) == true
            ? data.effect : null;
    }

    private void UpdateToolButton(ItemEffect effect)
    {
        if (_useToolButton == null) return;

        bool conditionMet = effect switch
        {
            FishEffect => _ctx.IsNearWater,
            MineEffect => _ctx.IsNearWall,
            _          => true
        };

        bool show = effect != null && conditionMet;
        _useToolButton.SetActive(show);

        if (show && _useToolHint != null)
            _useToolHint.text = effect.actionHint;
    }
}