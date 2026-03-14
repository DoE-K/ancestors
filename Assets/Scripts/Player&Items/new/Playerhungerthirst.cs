using UnityEngine;
using UnityEngine.UI;

public class PlayerHungerThirst : MonoBehaviour
{
    [Header("Decay Rates (per second)")]
    [SerializeField] private float _hungerDecayRate = 0.028f;
    [SerializeField] private float _thirstDecayRate = 0.083f;

    [Header("UI")]
    [SerializeField] private Slider _hungerSlider;
    [SerializeField] private Slider _thirstSlider;

    // ── Events ───────────────────────────────────────────────────────────────
    /// <summary>Fired once when the player dies of hunger or thirst.</summary>
    public System.Action OnDied;

    // ── Public read-only state ───────────────────────────────────────────────
    public float Hunger { get; private set; } = 100f;
    public float Thirst { get; private set; } = 100f;
    public bool IsDead  => Hunger <= 0f || Thirst <= 0f;

    // ── Private state ────────────────────────────────────────────────────────
    private bool _isDead = false;

    // ── Unity lifecycle ──────────────────────────────────────────────────────

    private void Start()
    {
        _hungerSlider.minValue = 0f;
        _hungerSlider.maxValue = 100f;
        _thirstSlider.minValue = 0f;
        _thirstSlider.maxValue = 100f;

        SyncSliders();
    }

    private void Update()
    {
        if (_isDead) return;

        Hunger = Mathf.Clamp(Hunger - _hungerDecayRate * Time.deltaTime, 0f, 100f);
        Thirst = Mathf.Clamp(Thirst - _thirstDecayRate * Time.deltaTime, 0f, 100f);

        SyncSliders();

        if (IsDead) Die();
    }

    // ── Public API ───────────────────────────────────────────────────────────

    /// <summary>Increases hunger (eating food). Amount should be positive.</summary>
    public void EatFood(float amount)
    {
        Hunger = Mathf.Clamp(Hunger + amount, 0f, 100f);
        _hungerSlider.value = Hunger;
    }

    /// <summary>Increases thirst (drinking water). Amount should be positive.</summary>
    public void DrinkWater(float amount)
    {
        Thirst = Mathf.Clamp(Thirst + amount, 0f, 100f);
        _thirstSlider.value = Thirst;
    }

    // ── Private helpers ──────────────────────────────────────────────────────

    private void SyncSliders()
    {
        _hungerSlider.value = Hunger;
        _thirstSlider.value = Thirst;
    }

    private void Die()
    {
        _isDead = true;
        Debug.Log("[PlayerHungerThirst] Spieler ist gestorben.");
        OnDied?.Invoke();
        // TODO: Hook OnDied up to a GameManager.HandlePlayerDeath() 
    }
}