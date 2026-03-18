using UnityEngine;

[CreateAssetMenu(fileName = "New Fruit", menuName = "Survival/Fruit Data")]
public class FruitData : ScriptableObject
{
    [Header("Fruit Prefab")]
    [Tooltip("The fruit prefab that grows on this source.")]
    public GameObject fruitPrefab;

    [Header("Display")]
    [Tooltip("Readable name used for debug logs.")]
    public string sourceName = "Fruit";
}