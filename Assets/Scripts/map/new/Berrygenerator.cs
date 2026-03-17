using UnityEngine;

[CreateAssetMenu(fileName = "New Bush", menuName = "Survival/Bush Data")]
public class BushData : ScriptableObject
{
    [Header("Berry Prefab")]
    [Tooltip("The berry prefab that grows on this bush type.")]
    public GameObject berryPrefab;

    [Header("Display")]
    [Tooltip("Readable name used for debug logs.")]
    public string bushName = "Bush";
}