using UnityEngine;

public class Animal : MonoBehaviour
{
    [Header("Species")]
    [SerializeField] private AnimalData _data;

    public AnimalData    Data    => _data;
    public AnimalNeeds   Needs   { get; private set; }
    public AnimalSensor  Sensor  { get; private set; }
    public AnimalBrain   Brain   { get; private set; }
    public AnimalMemory  Memory  { get; private set; }

    private Rigidbody2D    _rb;
    private AnimalSteering _steering;

    private void Awake()
    {
        Needs    = GetComponent<AnimalNeeds>();
        Sensor   = GetComponent<AnimalSensor>();
        Brain    = GetComponent<AnimalBrain>();
        Memory   = GetComponent<AnimalMemory>();
        _rb      = GetComponent<Rigidbody2D>();
        _steering = GetComponent<AnimalSteering>();

        if (_data == null)
        {
            Debug.LogError($"[Animal] '{name}' has no AnimalData assigned!", this);
            return;
        }

        Needs.Initialise(_data);
        Sensor.Initialise(_data);
        Brain.Initialise(this);
    }

    private void OnEnable()  => Needs.OnDied += HandleDeath;
    private void OnDisable() => Needs.OnDied -= HandleDeath;

    // ── Movement API ─────────────────────────────────────────────────────────

    public void MoveTowards(Vector2 target, float speed)
    {
        var dir = (target - (Vector2)transform.position).normalized;
        _rb.linearVelocity = dir * speed;
    }

    public void MoveTowardsWithAvoidance(Vector2 target, float speed)
    {
        if (_steering == null)
        {
            MoveTowards(target, speed);
            return;
        }

        _rb.linearVelocity = _steering.ComputeSteerDirection(
            transform.position, target, speed);
    }

    /// <summary>Stops all movement.</summary>
    public void StopMoving()
    {
        _rb.linearVelocity = Vector2.zero;
    }

    // ── Private ──────────────────────────────────────────────────────────────

    private void HandleDeath()
    {
        StopMoving();
        Destroy(gameObject, 0.5f);
    }
}