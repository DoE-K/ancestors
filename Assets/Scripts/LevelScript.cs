using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public float Level { get; private set; } = 100f;

    public void ChangeLevel(float amount)
    {
        Level = Mathf.Clamp(Level + amount, 0f, 100f);
    }
}
