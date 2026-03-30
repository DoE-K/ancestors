using UnityEngine;

[CreateAssetMenu(fileName = "New Cave Portal", menuName = "Survival/Cave Portal")]
public class CavePortal : ScriptableObject
{
    [Tooltip("Must match CaveScript.caveName exactly (e.g. 'cave0').")]
    public string caveName;

    [Tooltip("Where the player lands when entering this cave.")]
    public Vector2 entryDestination;

    [Tooltip("Where the player lands when exiting this cave.")]
    public Vector2 exitDestination;

    [Header("Required Item (optional)")]
    [Tooltip("If set, the player must hold this item in either hand to enter. " +
             "Leave empty for caves that are always accessible.")]
    public ItemData requiredItem;
}