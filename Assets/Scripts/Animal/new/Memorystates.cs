using UnityEngine;

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

    public void Enter()
    {
        _target = _memory.GetBestFoodMemory();
    }

    public void Execute()
    {
        if (_target == null) return;

        _animal.MoveTowards(_target.Value, _animal.Data.moveSpeed);

        // Check if arrived
        float dist = Vector2.Distance(_animal.transform.position, _target.Value);
        if (dist > _animal.Data.interactRange) return;

        // Arrived — is there actually food here?
        if (_animal.Sensor.CanSeeFood) return; // food found, EatState will take over

        // Nothing here — forget this stale location
        _memory.ForgetFoodAt(_target.Value);
        _target = _memory.GetBestFoodMemory(); // try next memory if any
    }

    public void Exit() { }
}

// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Navigates to the last remembered water position.
/// Forgets stale locations on arrival if no water is found.
/// </summary>
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

    public void Enter()
    {
        _target = _memory.GetBestWaterMemory();
    }

    public void Execute()
    {
        if (_target == null) return;

        _animal.MoveTowards(_target.Value, _animal.Data.moveSpeed);

        float dist = Vector2.Distance(_animal.transform.position, _target.Value);
        if (dist > _animal.Data.interactRange) return;

        if (_animal.Sensor.CanSeeWater) return;

        _memory.ForgetWaterAt(_target.Value);
        _target = _memory.GetBestWaterMemory();
    }

    public void Exit() { }
}