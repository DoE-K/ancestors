using UnityEngine;

/// <summary>
/// Detects when the player touches a CaveWall trigger.
/// </summary>
public class PlayerWallDetector : MonoBehaviour
{
    public bool IsNearWall { get; private set; } = false;

    private int _wallCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("CaveWall")) return;
        _wallCount++;
        IsNearWall = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("CaveWall")) return;
        _wallCount = Mathf.Max(0, _wallCount - 1);
        IsNearWall = _wallCount > 0;
    }
}