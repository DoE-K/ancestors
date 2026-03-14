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
}