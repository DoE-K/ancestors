using UnityEngine;

public class AnimalDebugInfo : MonoBehaviour
{
    [HideInInspector] public string CurrentState    = "—";
    [HideInInspector] public string PreviousState   = "—";
    [HideInInspector] public float  Hunger          = 100f;
    [HideInInspector] public float  Thirst          = 100f;
    [HideInInspector] public float  Energy          = 100f;
    [HideInInspector] public float  Age             = 0f;
    [HideInInspector] public float  MaxAge          = 600f;

    [HideInInspector] public bool   IsHungry        = false;
    [HideInInspector] public bool   IsThirsty       = false;
    [HideInInspector] public bool   IsTired         = false;
    [HideInInspector] public bool   IsReadyToMate   = false;
    [HideInInspector] public bool   CanSeeFood      = false;
    [HideInInspector] public bool   CanSeeWater     = false;
    [HideInInspector] public bool   CanSeePredator  = false;
    [HideInInspector] public bool   CanSeeMate      = false;
    [HideInInspector] public bool   IsNight         = false;
    [HideInInspector] public float  ReproduceCooldown = 0f;

    // Memory fields
    [HideInInspector] public bool HasFoodMemory   = false;
    [HideInInspector] public bool HasWaterMemory  = false;
    [HideInInspector] public int  FoodMemoryCount  = 0;
    [HideInInspector] public int  WaterMemoryCount = 0;
}