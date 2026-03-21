using UnityEngine;

[CreateAssetMenu(fileName = "New Animal", menuName = "Survival/Animal Data")]
public class AnimalData : ScriptableObject
{
    [Header("Identity")]
    public string speciesName = "Animal";

    [Header("Movement")]
    public float moveSpeed    = 3f;
    public float fleeSpeed    = 6f;
    public float wanderRadius = 5f;

    [Header("Senses")]
    public float sightRange   = 8f;
    public float interactRange = 1.2f;

    [Header("Needs — decay per second")]
    public float hungerDecayRate = 0.5f;
    public float thirstDecayRate = 0.8f;
    public float energyDecayRate = 0.3f;

    [Header("Needs — trigger thresholds (0–100)")]
    [Tooltip("Below this → start seeking food.")]
    public float hungryThreshold      = 40f;
    [Tooltip("Below this → start seeking water.")]
    public float thirstyThreshold     = 35f;
    [Tooltip("Below this → start sleeping.")]
    public float tiredThreshold       = 30f;

    [Header("Needs — stop thresholds (Hysteresis)")]
    [Tooltip("Above this → stop seeking food (must be higher than hungryThreshold).")]
    public float fullThreshold        = 80f;
    [Tooltip("Above this → stop seeking water.")]
    public float quenchedThreshold    = 75f;
    [Tooltip("Above this → wake up (must be higher than tiredThreshold).")]
    public float restedThreshold      = 90f;

    [Header("Needs — reproduce threshold")]
    [Tooltip("ALL needs must be above this to reproduce.")]
    public float reproduceThreshold   = 80f;

    [Header("Needs — restore amounts")]
    public float eatRestoreAmount   = 40f;
    public float drinkRestoreAmount = 50f;
    public float sleepRestoreRate   = 5f;

    [Header("Lifespan")]
    public float maxAge = 600f;

    [Header("Reproduction")]
    public float reproduceCooldown = 120f;

    [Header("Scene Tags")]
    public string homeTag     = "AnimalHome";
    public string waterTag    = "Water";
    public string predatorTag = "Player";
    public string foodTag     = "Fruit";
}