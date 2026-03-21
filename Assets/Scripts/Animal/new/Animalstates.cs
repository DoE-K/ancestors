using UnityEngine;

// ─────────────────────────────────────────────────────────────────────────────
// All FSM states for the animal AI.
// Each state has exactly one job. AnimalBrain decides which is active.
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Wanders slowly within a radius. Default state.</summary>
public class IdleState : IAnimalState
{
    private readonly Animal _animal;
    private Vector2 _wanderTarget;
    private float   _wanderTimer;

    public IdleState(Animal animal) => _animal = animal;

    public void Enter()
    {
        PickNewWanderTarget();
    }

    public void Execute()
    {
        _wanderTimer -= Time.deltaTime;
        if (_wanderTimer <= 0f) PickNewWanderTarget();

        _animal.MoveTowards(_wanderTarget, _animal.Data.moveSpeed * 0.5f);
    }

    public void Exit() { }

    private void PickNewWanderTarget()
    {
        var offset = Random.insideUnitCircle * _animal.Data.wanderRadius;
        _wanderTarget = (Vector2)_animal.transform.position + offset;
        _wanderTimer  = Random.Range(2f, 5f);
    }
}

// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Moves towards the nearest known food source.</summary>
public class SeekFoodState : IAnimalState
{
    private readonly Animal _animal;

    public SeekFoodState(Animal animal) => _animal = animal;

    public void Enter()  { }
    public void Exit()   { }

    public void Execute()
    {
        var food = _animal.Sensor.NearestFood;
        if (food == null) return;

        _animal.MoveTowards(food.position, _animal.Data.moveSpeed);
    }
}

// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Eats the nearest food item. Destroys the fruit GameObject so
/// FruitGenerator detects childCount == 0 and regrows automatically.
/// </summary>
public class EatState : IAnimalState
{
    private readonly Animal _animal;

    public EatState(Animal animal) => _animal = animal;

    public void Enter()
    {
        var food = _animal.Sensor.NearestFood;
        if (food == null) return;

        float dist = Vector2.Distance(_animal.transform.position, food.position);
        if (dist > _animal.Data.interactRange) return;

        // Destroy the fruit — FruitGenerator will regrow it automatically
        Object.Destroy(food.gameObject);
        _animal.Needs.Eat(_animal.Data.eatRestoreAmount);

        Debug.Log($"[EatState] {_animal.name} ate a fruit.");
    }

    public void Execute() { }
    public void Exit()    { }
}

// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Moves towards the nearest water source.</summary>
public class SeekWaterState : IAnimalState
{
    private readonly Animal _animal;

    public SeekWaterState(Animal animal) => _animal = animal;

    public void Enter()  { }
    public void Exit()   { }

    public void Execute()
    {
        var water = _animal.Sensor.NearestWater;
        if (water == null) return;

        _animal.MoveTowards(water.position, _animal.Data.moveSpeed);
    }
}

// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Drinks from the nearest water source when in range.</summary>
public class DrinkState : IAnimalState
{
    private readonly Animal _animal;

    public DrinkState(Animal animal) => _animal = animal;

    public void Enter()
    {
        var water = _animal.Sensor.NearestWater;
        if (water == null) return;

        float dist = Vector2.Distance(_animal.transform.position, water.position);
        if (dist > _animal.Data.interactRange) return;

        _animal.Needs.Drink(_animal.Data.drinkRestoreAmount);
        Debug.Log($"[DrinkState] {_animal.name} drank water.");
    }

    public void Execute() { }
    public void Exit()    { }
}

// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Flees away from the nearest predator at full speed.</summary>
public class FleeState : IAnimalState
{
    private readonly Animal _animal;

    public FleeState(Animal animal) => _animal = animal;

    public void Enter()  { }
    public void Exit()   { }

    public void Execute()
    {
        var predator = _animal.Sensor.NearestPredator;
        if (predator == null) return;

        // Move in the opposite direction from the predator
        var fleeDir  = ((Vector2)_animal.transform.position - (Vector2)predator.position).normalized;
        var fleeTarget = (Vector2)_animal.transform.position + fleeDir * 10f;
        _animal.MoveTowards(fleeTarget, _animal.Data.fleeSpeed);
    }
}

// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Navigates to the animal's home / shelter.</summary>
public class GoHomeState : IAnimalState
{
    private readonly Animal _animal;

    public GoHomeState(Animal animal) => _animal = animal;

    public void Enter()  { }
    public void Exit()   { }

    public void Execute()
    {
        var home = _animal.Sensor.NearestHome;
        if (home == null) return;

        _animal.MoveTowards(home.position, _animal.Data.moveSpeed);
    }
}

// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Stays at home and restores energy while it is night.</summary>
public class SleepState : IAnimalState
{
    private readonly Animal _animal;

    public SleepState(Animal animal) => _animal = animal;

    public void Enter()  => Debug.Log($"[SleepState] {_animal.name} is sleeping.");
    public void Exit()   => Debug.Log($"[SleepState] {_animal.name} woke up.");

    public void Execute()
    {
        _animal.Needs.RestoreEnergy();
    }
}

// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Moves toward a nearby mate. When in range, spawns an offspring
/// using the same AnimalData (with optional future mutation support).
/// </summary>
public class ReproduceState : IAnimalState
{
    private readonly Animal _animal;

    public ReproduceState(Animal animal) => _animal = animal;

    public void Enter()  { }
    public void Exit()   { }

    public void Execute()
    {
        var mate = _animal.Sensor.NearestMate;
        if (mate == null) return;

        float dist = Vector2.Distance(_animal.transform.position, mate.position);

        if (dist > _animal.Data.interactRange)
        {
            _animal.MoveTowards(mate.position, _animal.Data.moveSpeed);
            return;
        }

        SpawnOffspring();
        _animal.Brain.RecordReproduction();
    }

    private void SpawnOffspring()
    {
        // Spawn midpoint between the two parents
        var spawnPos = (Vector2)_animal.transform.position + Random.insideUnitCircle * 1.5f;
        var offspring = Object.Instantiate(_animal.gameObject, spawnPos, Quaternion.identity);

        // Reset the offspring's needs to full
        var needs = offspring.GetComponent<AnimalNeeds>();
        needs?.Initialise(_animal.Data);

        Debug.Log($"[ReproduceState] {_animal.name} spawned offspring.");
    }
}