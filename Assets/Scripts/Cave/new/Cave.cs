using UnityEngine;

public class Cave : MonoBehaviour
{
    [Tooltip("Must match the caveName in the corresponding CavePortal ScriptableObject.")]
    public string caveName;

    [Tooltip("If true, this trigger teleports the player back to the overworld.")]
    public bool isExit;
}