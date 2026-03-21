using System;
using UnityEngine;

public class AnimalNeeds : MonoBehaviour
{
    public event Action OnHungry;
    public event Action OnThirsty;
    public event Action OnTired;
    public event Action OnDied;
    public event Action OnReadyToReproduce;

    public float Hunger { get; private set; } = 100f;
    public float Thirst { get; private set; } = 100f;
    public float Energy { get; private set; } = 100f;
    public float Age    { get; private set; } = 0f;

    private bool _isHungry = false;
    private bool _isThirsty = false;
    private bool _isTired  = false;

    public bool IsHungry  => _isHungry;
    public bool IsThirsty => _isThirsty;
    public bool IsTired   => _isTired;

    public bool IsReadyToReproduce =>
        Hunger > _data.reproduceThreshold &&
        Thirst > _data.reproduceThreshold &&
        Energy > _data.reproduceThreshold;

    public bool IsDead => Hunger <= 0f || Thirst <= 0f || Age >= _data.maxAge;

    private AnimalData _data;
    private bool       _isDead = false;

    public void Initialise(AnimalData data)
    {
        _data = data;
    }

    private void Update()
    {
        if (_isDead || _data == null) return;

        DecayNeeds();
        UpdateHysteresisFlags();
        CheckDeath();
    }

    // ── Public API ───────────────────────────────────────────────────────────

    public void Eat(float amount)
    {
        Hunger = Mathf.Clamp(Hunger + amount, 0f, 100f);
    }

    public void Drink(float amount)
    {
        Thirst = Mathf.Clamp(Thirst + amount, 0f, 100f);
    }

    public void RestoreEnergy()
    {
        Energy = Mathf.Clamp(Energy + _data.sleepRestoreRate * Time.deltaTime, 0f, 100f);
    }

    // ── Private methods ──────────────────────────────────────────────────────

    private void DecayNeeds()
    {
        Hunger = Mathf.Clamp(Hunger - _data.hungerDecayRate * Time.deltaTime, 0f, 100f);
        Thirst = Mathf.Clamp(Thirst - _data.thirstDecayRate * Time.deltaTime, 0f, 100f);
        Energy = Mathf.Clamp(Energy - _data.energyDecayRate * Time.deltaTime, 0f, 100f);
        Age   += Time.deltaTime;
    }

    private void UpdateHysteresisFlags()
    {
        // Hunger: activate below hungryThreshold, deactivate above fullThreshold
        if (!_isHungry && Hunger < _data.hungryThreshold)
        {
            _isHungry = true;
            OnHungry?.Invoke();
        }
        else if (_isHungry && Hunger >= _data.fullThreshold)
        {
            _isHungry = false;
        }

        // Thirst: activate below thirstyThreshold, deactivate above quenchedThreshold
        if (!_isThirsty && Thirst < _data.thirstyThreshold)
        {
            _isThirsty = true;
            OnThirsty?.Invoke();
        }
        else if (_isThirsty && Thirst >= _data.quenchedThreshold)
        {
            _isThirsty = false;
        }

        // Energy: activate below tiredThreshold, deactivate above restedThreshold
        if (!_isTired && Energy < _data.tiredThreshold)
        {
            _isTired = true;
            OnTired?.Invoke();
        }
        else if (_isTired && Energy >= _data.restedThreshold)
        {
            _isTired = false;
        }
    }

    private void CheckDeath()
    {
        if (!IsDead) return;

        _isDead = true;

        string cause = Hunger <= 0f ? "hunger"
                     : Thirst <= 0f ? "thirst"
                     : "old age";

        Debug.Log($"[AnimalNeeds] {gameObject.name} died of {cause}.");
        OnDied?.Invoke();
    }
}