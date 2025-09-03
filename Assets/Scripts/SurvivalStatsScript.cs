using UnityEngine;

public class SurvivalStatsScript : MonoBehaviour
{
    public float Hunger { get; private set; } = 100f;
    public float Thirst { get; private set; } = 100f;

    public void ChangeHunger(float amount)
    {
        Hunger = Mathf.Clamp(Hunger + amount, 0f, 100f);
    }

    public void ChangeThirst(float amount)
    {
        Thirst = Mathf.Clamp(Thirst + amount, 0f, 100f);
    }

    public bool IsDead => Hunger <= 0 || Thirst <= 0;
}
