using UnityEngine;

public class AnimalSteering : MonoBehaviour
{
    [Header("Avoidance Settings")]
    [Tooltip("How far ahead the animal looks for obstacles.")]
    [SerializeField] private float _lookAheadDistance = 2.5f;

    [Tooltip("How strongly the animal steers away from obstacles (0–1).")]
    [SerializeField][Range(0f, 1f)] private float _avoidanceWeight = 0.8f;

    [Tooltip("Number of rays in the fan (more = smoother but more expensive).")]
    [SerializeField] private int _rayCount = 7;

    [Tooltip("Total angle of the ray fan in degrees.")]
    [SerializeField] private float _fanAngle = 120f;

    [Tooltip("Layers considered as obstacles (trees, bushes, rocks etc.).")]
    [SerializeField] private LayerMask _obstacleLayer;

    // ── Public API ────────────────────────────────────────────────────────────

    public Vector2 ComputeSteerDirection(Vector2 currentPos, Vector2 targetPos, float speed)
    {
        var desiredDir = (targetPos - currentPos).normalized;

        if (desiredDir == Vector2.zero) return Vector2.zero;

        var avoidance = ComputeAvoidance(currentPos, desiredDir);

        // Blend desired direction with avoidance
        var steerDir = (desiredDir + avoidance * _avoidanceWeight).normalized;

        return steerDir * speed;
    }

    // ── Private methods ───────────────────────────────────────────────────────

    private Vector2 ComputeAvoidance(Vector2 origin, Vector2 forward)
    {
        var avoidance   = Vector2.zero;
        int hitCount    = 0;
        float angleStep = _rayCount > 1 ? _fanAngle / (_rayCount - 1) : 0f;
        float startAngle = -_fanAngle / 2f;

        for (int i = 0; i < _rayCount; i++)
        {
            float angle  = startAngle + angleStep * i;
            var   rayDir = RotateVector(forward, angle);
            var   hit    = Physics2D.Raycast(origin, rayDir, _lookAheadDistance, _obstacleLayer);

            if (!hit) continue;

            // The closer the obstacle, the stronger the push away
            float proximity = 1f - (hit.distance / _lookAheadDistance);
            avoidance      += -rayDir * proximity;
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

    // ── Editor visualisation ──────────────────────────────────────────────────

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        var rb = GetComponent<Rigidbody2D>();
        if (rb == null || rb.linearVelocity == Vector2.zero) return;

        var forward     = rb.linearVelocity.normalized;
        float angleStep = _rayCount > 1 ? _fanAngle / (_rayCount - 1) : 0f;
        float startAngle = -_fanAngle / 2f;

        for (int i = 0; i < _rayCount; i++)
        {
            float angle  = startAngle + angleStep * i;
            var   rayDir = RotateVector(forward, angle);
            var   hit    = Physics2D.Raycast(transform.position, rayDir,
                               _lookAheadDistance, _obstacleLayer);

            Gizmos.color = hit ? Color.red : new Color(0f, 1f, 0f, 0.4f);
            Gizmos.DrawRay(transform.position, rayDir * _lookAheadDistance);
        }
    }
}