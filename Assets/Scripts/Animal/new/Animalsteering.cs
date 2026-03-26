using UnityEngine;

public class AnimalSteering : MonoBehaviour
{
    [Header("Avoidance Settings")]
    [SerializeField] private float _lookAheadDistance  = 3f;
    [SerializeField][Range(0f, 1f)] private float _avoidanceWeight = 0.85f;
    [SerializeField] private int   _rayCount           = 9;
    [SerializeField] private float _fanAngle           = 150f;
    [SerializeField] private LayerMask _obstacleLayer;

    [Header("Unstuck Settings")]
    [Tooltip("Seconds without meaningful movement before forcing a new direction.")]
    [SerializeField] private float _stuckTime          = 1.2f;
    [Tooltip("Minimum distance moved per second to be considered 'not stuck'.")]
    [SerializeField] private float _moveThreshold      = 0.15f;

    private Rigidbody2D _rb;
    private float       _stuckTimer     = 0f;
    private Vector2     _lastPos;
    private Vector2     _unstuckDir     = Vector2.zero;
    private float       _unstuckTimer   = 0f;
    private const float UnstuckDuration = 0.6f;

    private void Awake()
    {
        _rb      = GetComponent<Rigidbody2D>();
        _lastPos = transform.position;
    }

    private void Update()
    {
        CheckIfStuck();
    }

    // ── Public API ────────────────────────────────────────────────────────────

    public Vector2 ComputeSteerDirection(Vector2 currentPos, Vector2 targetPos, float speed)
    {
        var desiredDir = (targetPos - currentPos).normalized;
        if (desiredDir == Vector2.zero) return Vector2.zero;

        // If actively unstucking, follow that direction
        if (_unstuckTimer > 0f)
        {
            _unstuckTimer -= Time.deltaTime;
            return _unstuckDir * speed;
        }

        var avoidance = ComputeAvoidance(currentPos, desiredDir);
        var steerDir  = (desiredDir + avoidance * _avoidanceWeight).normalized;

        return steerDir * speed;
    }

    // ── Unstuck logic ─────────────────────────────────────────────────────────

    private void CheckIfStuck()
    {
        if (_rb == null) return;

        float moved = Vector2.Distance((Vector2)transform.position, _lastPos);
        _lastPos    = transform.position;

        bool isMoving = _rb.linearVelocity.magnitude > 0.1f;

        if (isMoving && moved < _moveThreshold * Time.deltaTime * 60f)
        {
            _stuckTimer += Time.deltaTime;

            if (_stuckTimer >= _stuckTime)
            {
                // Pick a random perpendicular direction to break free
                float angle   = Random.Range(60f, 120f) * (Random.value > 0.5f ? 1f : -1f);
                _unstuckDir   = RotateVector(_rb.linearVelocity.normalized, angle).normalized;
                _unstuckTimer = UnstuckDuration;
                _stuckTimer   = 0f;
            }
        }
        else
        {
            _stuckTimer = 0f;
        }
    }

    // ── Avoidance ─────────────────────────────────────────────────────────────

    private Vector2 ComputeAvoidance(Vector2 origin, Vector2 forward)
    {
        var   avoidance  = Vector2.zero;
        int   hitCount   = 0;
        float angleStep  = _rayCount > 1 ? _fanAngle / (_rayCount - 1) : 0f;
        float startAngle = -_fanAngle / 2f;

        for (int i = 0; i < _rayCount; i++)
        {
            float angle  = startAngle + angleStep * i;
            var   rayDir = RotateVector(forward, angle);
            var   hit    = Physics2D.Raycast(origin, rayDir, _lookAheadDistance, _obstacleLayer);

            if (!hit) continue;

            float proximity = 1f - (hit.distance / _lookAheadDistance);

            // Center ray (straight ahead) gets double weight — direct block = hard steer
            float rayWeight = (Mathf.Abs(angle) < angleStep * 0.5f) ? 2f : 1f;

            avoidance += -rayDir * proximity * rayWeight;
            hitCount++;
        }

        return hitCount > 0 ? avoidance.normalized : Vector2.zero;
    }

    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(cos * v.x - sin * v.y, sin * v.x + cos * v.y);
    }

    // ── Gizmos ────────────────────────────────────────────────────────────────

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || _rb == null) return;
        if (_rb.linearVelocity == Vector2.zero) return;

        var   forward    = _rb.linearVelocity.normalized;
        float angleStep  = _rayCount > 1 ? _fanAngle / (_rayCount - 1) : 0f;
        float startAngle = -_fanAngle / 2f;

        for (int i = 0; i < _rayCount; i++)
        {
            float angle  = startAngle + angleStep * i;
            var   rayDir = RotateVector(forward, angle);
            var   hit    = Physics2D.Raycast(transform.position, rayDir,
                               _lookAheadDistance, _obstacleLayer);

            Gizmos.color = hit ? Color.red : new Color(0f, 1f, 0f, 0.35f);
            Gizmos.DrawRay(transform.position, rayDir * _lookAheadDistance);
        }

        // Show unstuck direction in blue
        if (_unstuckTimer > 0f)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, _unstuckDir * 2f);
        }
    }
}