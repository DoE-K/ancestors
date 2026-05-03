using UnityEngine;

/// <summary>
/// Snapshot of player state passed to every ItemEffect.
/// </summary>
public class PlayerContext
{
    public Transform           Transform;
    public PlayerInventory     Inventory;
    public CameraZoom          Camera;
    public DayNight            DayNight;
    public PlayerWaterDetector WaterDetector;
    public PlayerWallDetector  WallDetector;

    public Vector2 Position    => Transform.position;
    public bool    IsNearWater => WaterDetector != null && WaterDetector.IsNearWater;
    public bool    IsNearWall  => WallDetector  != null && WallDetector.IsNearWall;
}