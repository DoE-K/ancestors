using UnityEngine;

/// <summary>
/// Simple MonoBehaviour wrapper around the ItemRegistry ScriptableObject.
/// Place on any persistent GameObject (e.g. GameManager).
/// Allows other scripts to find the registry via FindAnyObjectByType.
/// </summary>
public class ItemRegistryHolder : MonoBehaviour
{
    [SerializeField] private ItemRegistry _registry;
    public ItemRegistry Registry => _registry;
}