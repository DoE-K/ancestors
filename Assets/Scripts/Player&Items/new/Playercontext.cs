using UnityEngine;

/// <summary>
/// Snapshot of player state passed to every ItemEffect.
/// Add new fields here as more tools need access to more systems.
/// </summary>
public class PlayerContext
{
    public Transform       Transform;
    public PlayerInventory Inventory;
    public CameraZoom      Camera;
    public DayNight        DayNight;

    // Convenience
    public Vector2 Position => Transform.position;
}