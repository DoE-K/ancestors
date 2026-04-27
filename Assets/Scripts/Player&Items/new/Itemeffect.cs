using UnityEngine;

/// <summary>
/// Abstract base for all tool effects.
/// Create a subclass for each tool behaviour.
///
/// Example:
///   [CreateAssetMenu(menuName = "Survival/Effects/Mine")]
///   public class MineEffect : ItemEffect { ... }
///
/// Then create an asset in the Project, assign it to an ItemData.effect field.
/// No code change in PlayerToolHandler ever needed for new tools.
/// </summary>
public abstract class ItemEffect : ScriptableObject
{
    [Tooltip("Shown in the UI as a hint when player holds this tool.")]
    public string actionHint = "Use";

    /// <summary>
    /// Called when the player activates this tool.
    /// ctx gives access to all player systems the effect might need.
    /// </summary>
    public abstract void Use(PlayerContext ctx);

    /// <summary>
    /// Optional: called every frame while the item is held.
    /// Override for continuous effects (e.g. Telescope keeping camera zoomed).
    /// Default implementation does nothing.
    /// </summary>
    public virtual void WhileHeld(PlayerContext ctx) { }

    /// <summary>
    /// Optional: called when the item leaves the hand.
    /// Override to clean up continuous effects.
    /// </summary>
    public virtual void OnUnequip(PlayerContext ctx) { }
}