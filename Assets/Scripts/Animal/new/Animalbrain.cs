using UnityEngine;

/// <summary>
/// Animal decision-making with spatial memory fallback.
///
/// Priority order:
///   1. Flee           — predator within safe distance
///   2. Eat            — food in interact range and hungry
///   3. SeekFood       — hungry, food currently visible
///   4. SeekMemoryFood — hungry, no food visible, but remembered a location  ← NEW
///   5. Drink          — water in interact range and thirsty
///   6. SeekWater      — thirsty, water currently visible
///   7. SeekMemoryWater— thirsty, no water visible, but remembered a location ← NEW
///   8. Reproduce      — all needs high, mate visible, cooldown elapsed
///   9. GoHome         — night, home visible
///  10. Sleep          — night and tired
///  11. Idle           — default
/// </summary>
public class AnimalBrain : MonoBehaviour
{
    public void RecordReproduction() => _reproduceCooldownTimer = _animal.Data.reproduceCooldown;

    private Animal          _animal;
    private AnimalMemory    _memory;
    private AnimalDebugInfo _debug;
    private IAnimalState    _currentState;
    private float           _reproduceCooldownTimer = 0f;
    private DayNight        _dayNight;

    private float SafeFleeDistance => _animal.Data.sightRange * 1.5f;

    private IdleState            _idle;
    private SeekFoodState        _seekFood;
    private EatState             _eat;
    private SeekWaterState       _seekWater;
    private DrinkState           _drink;
    private FleeState            _flee;
    private GoHomeState          _goHome;
    private SleepState           _sleep;
    private ReproduceState       _reproduce;
    private SeekMemoryFoodState  _seekMemoryFood;
    private SeekMemoryWaterState _seekMemoryWater;

    public void Initialise(Animal animal)
    {
        _animal   = animal;
        _memory   = GetComponent<AnimalMemory>();
        _debug    = GetComponent<AnimalDebugInfo>();
        _dayNight = FindAnyObjectByType<DayNight>();

        if (_debug != null) _debug.MaxAge = animal.Data.maxAge;

        _idle            = new IdleState(animal);
        _seekFood        = new SeekFoodState(animal);
        _eat             = new EatState(animal);
        _seekWater       = new SeekWaterState(animal);
        _drink           = new DrinkState(animal);
        _flee            = new FleeState(animal);
        _goHome          = new GoHomeState(animal);
        _sleep           = new SleepState(animal);
        _reproduce       = new ReproduceState(animal);
        _seekMemoryFood  = new SeekMemoryFoodState(animal, _memory);
        _seekMemoryWater = new SeekMemoryWaterState(animal, _memory);

        SwitchState(_idle);
    }

    private void Update()
    {
        if (_animal == null) return;

        _reproduceCooldownTimer -= Time.deltaTime;

        var next = EvaluatePriority();
        if (next != _currentState) SwitchState(next);

        _currentState?.Execute();
        UpdateDebugInfo();
    }

    private IAnimalState EvaluatePriority()
    {
        var needs  = _animal.Needs;
        var sensor = _animal.Sensor;
        var data   = _animal.Data;

        // 1. Flee
        if (sensor.CanSeePredator)
        {
            float dist = Vector2.Distance(transform.position, sensor.NearestPredator.position);
            if (dist < SafeFleeDistance) return _flee;
        }

        // 2 + 3 + 4. Hunger chain
        if (needs.IsHungry)
        {
            if (sensor.CanSeeFood)
            {
                float dist = Vector2.Distance(transform.position, sensor.NearestFood.position);
                return dist <= data.interactRange ? _eat : _seekFood;
            }

            // Memory fallback — only if memory system is present
            if (_memory != null && _memory.HasFoodMemory)
                return _seekMemoryFood;
        }

        // 5 + 6 + 7. Thirst chain
        if (needs.IsThirsty)
        {
            if (sensor.CanSeeWater)
            {
                float dist = Vector2.Distance(transform.position, sensor.NearestWater.position);
                return dist <= data.interactRange ? _drink : _seekWater;
            }

            if (_memory != null && _memory.HasWaterMemory)
                return _seekMemoryWater;
        }

        // 8. Reproduce
        if (needs.IsReadyToReproduce && sensor.CanSeeMate && _reproduceCooldownTimer <= 0f)
            return _reproduce;

        // 9 + 10. Night behaviour
        if (IsNight() && sensor.CanSeeHome) return _goHome;
        if (IsNight() && needs.IsTired)     return _sleep;

        // 11. Default
        return _idle;
    }

    private void SwitchState(IAnimalState next)
    {
        if (_debug != null) _debug.PreviousState = _debug.CurrentState;
        _currentState?.Exit();
        _animal.StopMoving();
        _currentState = next;
        _currentState?.Enter();
        if (_debug != null) _debug.CurrentState = next?.GetType().Name ?? "—";
    }

    private void UpdateDebugInfo()
    {
        if (_debug == null) return;
        var needs  = _animal.Needs;
        var sensor = _animal.Sensor;

        _debug.Hunger         = needs.Hunger;
        _debug.Thirst         = needs.Thirst;
        _debug.Energy         = needs.Energy;
        _debug.Age            = needs.Age;
        _debug.IsHungry       = needs.IsHungry;
        _debug.IsThirsty      = needs.IsThirsty;
        _debug.IsTired        = needs.IsTired;
        _debug.IsReadyToMate  = needs.IsReadyToReproduce;
        _debug.CanSeeFood     = sensor.CanSeeFood;
        _debug.CanSeeWater    = sensor.CanSeeWater;
        _debug.CanSeePredator = sensor.CanSeePredator;
        _debug.CanSeeMate     = sensor.CanSeeMate;
        _debug.IsNight        = IsNight();
        _debug.ReproduceCooldown = Mathf.Max(0f, _reproduceCooldownTimer);

        // Memory info for debug panel
        _debug.HasFoodMemory  = _memory != null && _memory.HasFoodMemory;
        _debug.HasWaterMemory = _memory != null && _memory.HasWaterMemory;
        _debug.FoodMemoryCount  = _memory?.FoodMemories.Count  ?? 0;
        _debug.WaterMemoryCount = _memory?.WaterMemories.Count ?? 0;
    }

    private bool IsNight() => _dayNight != null && _dayNight.IsNight;
}