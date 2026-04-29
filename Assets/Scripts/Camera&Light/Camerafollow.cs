using UnityEngine;

/// <summary>
/// Soft follow camera for top-down 2D.
///
/// Behaviour:
///   - The camera stays still while the player is within a central "dead zone"
///   - Once the player approaches the edge of the dead zone, the camera
///     smoothly follows to keep the player visible
///   - Movement is lerped for a natural, delayed feel
///
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _player;

    [Header("Dead Zone")]
    [Tooltip("How far the player can move from the camera center before it starts following. " +
             "In world units — try 2–4 for a top-down game.")]
    [SerializeField] private float _deadZoneRadius = 3f;

    [Header("Follow Speed")]
    [Tooltip("How fast the camera catches up. Lower = more delayed. Try 2–5.")]
    [SerializeField] private float _followSpeed = 3f;

    [Header("Camera Depth")]
    [Tooltip("Z position of the camera. Keep at -10 for standard 2D.")]
    [SerializeField] private float _cameraZ = -10f;

    // ── Unity lifecycle ───────────────────────────────────────────────────────

    private void LateUpdate()
    {
        if (_player == null) return;

        Vector2 camPos    = transform.position;
        Vector2 playerPos = _player.position;

        float dist = Vector2.Distance(camPos, playerPos);

        // Only move if player is outside the dead zone
        if (dist <= _deadZoneRadius) return;

        // Target is just far enough to bring the player back to the dead zone edge
        Vector2 dir    = (playerPos - camPos).normalized;
        Vector2 target = playerPos - dir * _deadZoneRadius;

        Vector2 newPos = Vector2.Lerp(camPos, target, _followSpeed * Time.deltaTime);

        transform.position = new Vector3(newPos.x, newPos.y, _cameraZ);
    }

    // ── Editor visualisation ──────────────────────────────────────────────────

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, _deadZoneRadius);
    }
}