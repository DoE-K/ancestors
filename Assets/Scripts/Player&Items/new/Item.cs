using UnityEngine;

public class Item : MonoBehaviour
{
    [Tooltip("Drag the matching ItemData ScriptableObject here.")]
    public ItemData data;

    public string ItemName => data != null ? data.itemName : string.Empty;
    public bool AbleToPickup => data != null && data.ableToPickup;
    public bool DestroyOnPickup => data == null || data.destroyOnPickup;
}