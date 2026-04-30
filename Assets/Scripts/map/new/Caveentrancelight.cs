using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Simulates sunlight streaming into a cave entrance.
/// Uses a Spot Light 2D (or Freeform) positioned at the entrance.
/// Intensity follows the time of day — no light at night.
/// </summary>
public class CaveEntranceLight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light2D  _light;
    [SerializeField] private DayNight _dayNight;

    [Header("Day Settings")]
    [SerializeField] private float _maxIntensity = 1.5f;
    [SerializeField] private Color _dayColor     = new Color(1f, 0.95f, 0.8f);

    [Header("Transition")]
    [SerializeField] private float _fadeSpeed    = 1f;

    private float _currentIntensity = 0f;

    private void Awake()
    {
        if (_light != null) _light.color = _dayColor;
    }

    private void Update()
    {
        if (_light == null || _dayNight == null) return;

        // Light follows the sun — zero at night, max at noon
        float hour          = _dayNight.CurrentHour;
        float targetIntensity = _dayNight.IsNight ? 0f
            : Mathf.Sin(Mathf.Clamp01((hour - 6f) / 12f) * Mathf.PI) * _maxIntensity;

        _currentIntensity = Mathf.Lerp(
            _currentIntensity, targetIntensity, _fadeSpeed * Time.deltaTime);

        _light.intensity = _currentIntensity;
    }
}