using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Controls the GlobalLight2D to simulate realistic sky lighting over 24h.
///
/// Time phases:
///   Night      → dark blue,   intensity 0.10
///   Dawn       → orange-pink, intensity 0.4  (sunrise transition)
///   Day        → warm white,  intensity 1.0
///   Dusk       → orange-red,  intensity 0.5  (sunset transition)
///   Night      → dark blue again
/// </summary>
public class DayNightLighting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light2D _globalLight;
    [SerializeField] private DayNight _dayNight;

    [Header("Light Colors")]
    [SerializeField] private Color _nightColor  = new Color(0.05f, 0.07f, 0.18f);
    [SerializeField] private Color _dawnColor   = new Color(1.0f,  0.55f, 0.25f);
    [SerializeField] private Color _dayColor    = new Color(1.0f,  0.97f, 0.88f);
    [SerializeField] private Color _duskColor   = new Color(1.0f,  0.40f, 0.15f);

    [Header("Light Intensities")]
    [SerializeField] private float _nightIntensity = 0.10f;
    [SerializeField] private float _dawnIntensity  = 0.45f;
    [SerializeField] private float _dayIntensity   = 1.00f;
    [SerializeField] private float _duskIntensity  = 0.50f;

    [Header("Transition Hours")]
    [SerializeField] private float _sunriseHour    = 6f;
    [SerializeField] private float _sunsetHour     = 18f;
    [SerializeField] private float _transitionHours = 1.5f;

    private void Update()
    {
        if (_globalLight == null || _dayNight == null) return;

        float hour = GetCurrentHour();
        _globalLight.color     = ComputeColor(hour);
        _globalLight.intensity = ComputeIntensity(hour);
    }

    private float GetCurrentHour()
    {
        return _dayNight.CurrentHour;
    }

    private Color ComputeColor(float hour)
    {
        float sr = _sunriseHour;
        float ss = _sunsetHour;
        float tr = _transitionHours;

        // Night → Dawn
        if (hour >= sr - tr && hour < sr)
            return Color.Lerp(_nightColor, _dawnColor,
                Mathf.InverseLerp(sr - tr, sr, hour));

        // Dawn → Day
        if (hour >= sr && hour < sr + tr)
            return Color.Lerp(_dawnColor, _dayColor,
                Mathf.InverseLerp(sr, sr + tr, hour));

        // Day
        if (hour >= sr + tr && hour < ss - tr)
            return _dayColor;

        // Day → Dusk
        if (hour >= ss - tr && hour < ss)
            return Color.Lerp(_dayColor, _duskColor,
                Mathf.InverseLerp(ss - tr, ss, hour));

        // Dusk → Night
        if (hour >= ss && hour < ss + tr)
            return Color.Lerp(_duskColor, _nightColor,
                Mathf.InverseLerp(ss, ss + tr, hour));

        return _nightColor;
    }

    private float ComputeIntensity(float hour)
    {
        float sr = _sunriseHour;
        float ss = _sunsetHour;
        float tr = _transitionHours;

        if (hour >= sr - tr && hour < sr)
            return Mathf.Lerp(_nightIntensity, _dawnIntensity,
                Mathf.InverseLerp(sr - tr, sr, hour));

        if (hour >= sr && hour < sr + tr)
            return Mathf.Lerp(_dawnIntensity, _dayIntensity,
                Mathf.InverseLerp(sr, sr + tr, hour));

        if (hour >= sr + tr && hour < ss - tr)
            return _dayIntensity;

        if (hour >= ss - tr && hour < ss)
            return Mathf.Lerp(_dayIntensity, _duskIntensity,
                Mathf.InverseLerp(ss - tr, ss, hour));

        if (hour >= ss && hour < ss + tr)
            return Mathf.Lerp(_duskIntensity, _nightIntensity,
                Mathf.InverseLerp(ss, ss + tr, hour));

        return _nightIntensity;
    }
}