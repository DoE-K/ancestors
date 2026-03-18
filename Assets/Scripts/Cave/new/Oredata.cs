using UnityEngine;

[CreateAssetMenu(fileName = "New Ore", menuName = "Survival/Ore Data")]
public class OreData : ScriptableObject
{
    [Tooltip("The ore prefab to spawn.")]
    public GameObject prefab;

    [Tooltip("Relative spawn weight. Higher = more common. E.g. Stone:40, Gold:15, Diamond:5.")]
    [Min(0f)]
    public float weight = 10f;
}