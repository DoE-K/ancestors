using UnityEngine;

public class PlayerWaterDetector : MonoBehaviour
{
    public bool IsNearWater { get; private set; } = false;

    private int _waterCount = 0; // counts overlapping water triggers

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Water")) return;
        _waterCount++;
        IsNearWater = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Water")) return;
        _waterCount = Mathf.Max(0, _waterCount - 1);
        IsNearWater = _waterCount > 0;
    }
}