using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Controls a Point Light 2D on the player.
/// Fades in at night, subtle flicker, configurable falloff.
/// </summary>
public class PlayerLight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light2D  _light;
    [SerializeField] private DayNight _dayNight;

    [Header("Night Settings")]
    [SerializeField] private float _targetIntensity = 1.0f;
    [SerializeField] private float _targetRadius    = 5f;
    [SerializeField] private Color _lightColor      = new Color(1f, 0.85f, 0.6f);

    [Header("Falloff")]
    [Tooltip("Light falloff strength (0 = no falloff, 1 = sharp edge). Default 0.8")]
    [SerializeField][Range(0f, 1f)] private float _falloffStrength = 0.8f;

    [Header("Flicker (subtle)")]
    [SerializeField] private float _flickerAmount = 0.05f;
    [SerializeField] private float _flickerSpeed  = 2f;

    [Header("Transition")]
    [SerializeField] private float _fadeSpeed = 2f;

    private float _currentIntensity = 0f;
    private float _seed;

    private void Awake()
    {
        _seed = Random.Range(0f, 100f);
        if (_light == null) return;
        _light.color        = _lightColor;
        _light.falloffIntensity = _falloffStrength;
    }

    private void Update()
    {
        if (_light == null || _dayNight == null) return;

        float target = _dayNight.IsNight ? _targetIntensity : 0f;

        _currentIntensity = Mathf.Lerp(
            _currentIntensity, target, _fadeSpeed * Time.deltaTime);

        float flicker = Mathf.PerlinNoise(Time.time * _flickerSpeed, _seed) - 0.5f;

        _light.intensity             = _currentIntensity + flicker * _flickerAmount;
        _light.pointLightOuterRadius = _targetRadius;
        _light.falloffIntensity      = _falloffStrength;
    }
}