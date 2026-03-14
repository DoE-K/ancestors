using UnityEngine;

/// <summary>
/// e.g. "Visit the BallRun Memorial"
/// </summary>
[CreateAssetMenu(fileName = "New Location Achievement", menuName = "Survival/Location Achievement")]
public class LocationAchievement : ScriptableObject
{
    [Tooltip("Must match the GameObject tag in the scene exactly.")]
    public string targetTag;

    public int scoreReward = 100;

    [Tooltip("Display name shown in UI (optional).")]
    public string displayName;
}