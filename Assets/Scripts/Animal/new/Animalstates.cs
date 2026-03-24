using UnityEngine;

public class IdleState : IAnimalState
{
    private readonly Animal _animal;
    private Vector2 _wanderTarget;
    private float   _wanderTimer;

    public IdleState(Animal animal) => _animal = animal;

    public void Enter() => PickNewWanderTarget();
    public void Exit()  { }

    public void Execute()
    {
        _wanderTimer -= Time.deltaTime;
        if (_wanderTimer <= 0f) PickNewWanderTarget();

        _animal.MoveTowardsWithAvoidance(_wanderTarget, _animal.Data.moveSpeed * 0.5f);
    }

    private void PickNewWanderTarget()
    {
        var offset = Random.insideUnitCircle * _animal.Data.wanderRadius;
        _wanderTarget = (Vector2)_animal.transform.position + offset;
        _wanderTimer  = Random.Range(2f, 5f);
    }
}

// ─────────────────────────────────────────────────────────────────────────────

public class SeekFoodState : IAnimalState
{
    private readonly Animal _animal;
    public SeekFoodState(Animal animal) => _animal = animal;
    public void Enter() { }
    public void Exit()  { }

    public void Execute()
    {
        var food = _animal.Sensor.NearestFood;
        if (food == null) return;
        _animal.MoveTowardsWithAvoidance(food.position, _animal.Data.moveSpeed);
    }
}

// ─────────────────────────────────────────────────────────────────────────────

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

        Object.Destroy(food.gameObject);
        _animal.Needs.Eat(_animal.Data.eatRestoreAmount);
    }

    public void Execute() { }
    public void Exit()    { }
}

// ─────────────────────────────────────────────────────────────────────────────

public class SeekWaterState : IAnimalState
{
    private readonly Animal _animal;
    public SeekWaterState(Animal animal) => _animal = animal;
    public void Enter() { }
    public void Exit()  { }

    public void Execute()
    {
        var water = _animal.Sensor.NearestWater;
        if (water == null) return;
        _animal.MoveTowardsWithAvoidance(water.position, _animal.Data.moveSpeed);
    }
}

// ─────────────────────────────────────────────────────────────────────────────

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
    }

    public void Execute() { }
    public void Exit()    { }
}

// ─────────────────────────────────────────────────────────────────────────────

public class FleeState : IAnimalState
{
    private readonly Animal _animal;
    public FleeState(Animal animal) => _animal = animal;
    public void Enter() { }
    public void Exit()  { }

    public void Execute()
    {
        var predator = _animal.Sensor.NearestPredator;
        if (predator == null) return;

        var fleeDir    = ((Vector2)_animal.transform.position - (Vector2)predator.position).normalized;
        var fleeTarget = (Vector2)_animal.transform.position + fleeDir * 10f;
        _animal.MoveTowards(fleeTarget, _animal.Data.fleeSpeed);
    }
}

// ─────────────────────────────────────────────────────────────────────────────

public class GoHomeState : IAnimalState
{
    private readonly Animal _animal;
    public GoHomeState(Animal animal) => _animal = animal;
    public void Enter() { }
    public void Exit()  { }

    public void Execute()
    {
        var home = _animal.Sensor.NearestHome;
        if (home == null) return;
        _animal.MoveTowardsWithAvoidance(home.position, _animal.Data.moveSpeed);
    }
}

// ─────────────────────────────────────────────────────────────────────────────

public class SleepState : IAnimalState
{
    private readonly Animal _animal;
    public SleepState(Animal animal) => _animal = animal;
    public void Enter() { }
    public void Exit()  { }
    public void Execute() => _animal.Needs.RestoreEnergy();
}

// ─────────────────────────────────────────────────────────────────────────────

public class ReproduceState : IAnimalState
{
    private readonly Animal _animal;
    public ReproduceState(Animal animal) => _animal = animal;
    public void Enter() { }
    public void Exit()  { }

    public void Execute()
    {
        var mate = _animal.Sensor.NearestMate;
        if (mate == null) return;

        float dist = Vector2.Distance(_animal.transform.position, mate.position);

        if (dist > _animal.Data.interactRange)
        {
            _animal.MoveTowardsWithAvoidance(mate.position, _animal.Data.moveSpeed);
            return;
        }

        SpawnOffspring();
        _animal.Brain.RecordReproduction();
    }

    private void SpawnOffspring()
    {
        var spawnPos = (Vector2)_animal.transform.position + Random.insideUnitCircle * 1.5f;
        var offspring = Object.Instantiate(_animal.gameObject, spawnPos, Quaternion.identity);
        offspring.GetComponent<AnimalNeeds>()?.Initialise(_animal.Data);
    }
}

// ─────────────────────────────────────────────────────────────────────────────

public class SeekMemoryFoodState : IAnimalState
{
    private readonly Animal       _animal;
    private readonly AnimalMemory _memory;
    private Vector2?              _target;

    public SeekMemoryFoodState(Animal animal, AnimalMemory memory)
    {
        _animal = animal;
        _memory = memory;
    }

    public void Enter()  => _target = _memory.GetBestFoodMemory();
    public void Exit()   { }

    public void Execute()
    {
        if (_target == null) return;

        _animal.MoveTowardsWithAvoidance(_target.Value, _animal.Data.moveSpeed);

        float dist = Vector2.Distance(_animal.transform.position, _target.Value);
        if (dist > _animal.Data.interactRange) return;

        if (_animal.Sensor.CanSeeFood) return;

        _memory.ForgetFoodAt(_target.Value);
        _target = _memory.GetBestFoodMemory();
    }
}

// ─────────────────────────────────────────────────────────────────────────────

public class SeekMemoryWaterState : IAnimalState
{
    private readonly Animal       _animal;
    private readonly AnimalMemory _memory;
    private Vector2?              _target;

    public SeekMemoryWaterState(Animal animal, AnimalMemory memory)
    {
        _animal = animal;
        _memory = memory;
    }

    public void Enter()  => _target = _memory.GetBestWaterMemory();
    public void Exit()   { }

    public void Execute()
    {
        if (_target == null) return;

        _animal.MoveTowardsWithAvoidance(_target.Value, _animal.Data.moveSpeed);

        float dist = Vector2.Distance(_animal.transform.position, _target.Value);
        if (dist > _animal.Data.interactRange) return;

        if (_animal.Sensor.CanSeeWater) return;

        _memory.ForgetWaterAt(_target.Value);
        _target = _memory.GetBestWaterMemory();
    }
}