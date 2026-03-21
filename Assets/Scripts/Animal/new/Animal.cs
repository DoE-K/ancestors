using UnityEngine;

public class Animal : MonoBehaviour
{
    [Header("Species")]
    [SerializeField] private AnimalData _data;

    // ── Public references (read by states) ───────────────────────────────────
    public AnimalData   Data   => _data;
    public AnimalNeeds  Needs  { get; private set; }
    public AnimalSensor Sensor { get; private set; }
    public AnimalBrain  Brain  { get; private set; }

    private Rigidbody2D _rb;

    // ── Unity lifecycle ──────────────────────────────────────────────────────

    private void Awake()
    {
        Needs  = GetComponent<AnimalNeeds>();
        Sensor = GetComponent<AnimalSensor>();
        Brain  = GetComponent<AnimalBrain>();
        _rb    = GetComponent<Rigidbody2D>();

        if (_data == null)
        {
            Debug.LogError($"[Animal] '{name}' has no AnimalData assigned!", this);
            return;
        }

        Needs.Initialise(_data);
        Sensor.Initialise(_data);
        Brain.Initialise(this);
    }

    private void OnEnable()
    {
        Needs.OnDied += HandleDeath;
    }

    private void OnDisable()
    {
        Needs.OnDied -= HandleDeath;
    }

    // ── Public movement API (used by states) ─────────────────────────────────

    /// <summary>Moves the animal toward a world position at the given speed.</summary>
    public void MoveTowards(Vector2 target, float speed)
    {
        var dir = (target - (Vector2)transform.position).normalized;
        _rb.linearVelocity = dir * speed;
    }

    /// <summary>Stops all movement.</summary>
    public void StopMoving()
    {
        _rb.linearVelocity = Vector2.zero;
    }

    // ── Private handlers ─────────────────────────────────────────────────────

    private void HandleDeath()
    {
        StopMoving();
        // TODO: play death animation, drop loot (hide, bone)
        Destroy(gameObject, 0.5f);
    }
}