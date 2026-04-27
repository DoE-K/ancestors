using UnityEngine;

/// <summary>
/// Tool effect for telescope / spyglass.
/// While held: camera zooms out to a wider view.
/// On unequip: camera returns to normal zoom.
/// No button press needed — effect is continuous (WhileHeld).
///
/// Create via: Right-click → Create → Survival/Effects/Telescope
/// Assign to: Teleskop_ItemData
/// </summary>
[CreateAssetMenu(fileName = "TelescopeEffect", menuName = "Survival/Effects/Telescope")]
public class TelescopeEffect : ItemEffect
{
    [Tooltip("Camera orthographic size while telescope is held. " +
             "Larger = more zoomed out.")]
    public float zoomedOutSize = 12f;

    private bool _applied = false;

    public override void Use(PlayerContext ctx)
    {
        // Toggle on button press
        _applied = !_applied;
        ApplyZoom(ctx, _applied);
    }

    public override void WhileHeld(PlayerContext ctx)
    {
        // Keep zoom applied as long as held (after Use() activated it)
    }

    public override void OnUnequip(PlayerContext ctx)
    {
        _applied = false;
        ctx.Camera?.SetTarget(CameraZoom.ZoomLevel.Far);
        Debug.Log("[TelescopeEffect] Zoom zurückgesetzt.");
    }

    private void ApplyZoom(PlayerContext ctx, bool active)
    {
        if (ctx.Camera == null) return;

        if (active)
        {
            // Temporarily set a custom size — extend CameraZoom if needed
            ctx.Camera.SetCustomSize(zoomedOutSize);
            Debug.Log("[TelescopeEffect] Fernrohr aktiviert.");
        }
        else
        {
            ctx.Camera.SetTarget(CameraZoom.ZoomLevel.Far);
            Debug.Log("[TelescopeEffect] Fernrohr deaktiviert.");
        }
    }
}