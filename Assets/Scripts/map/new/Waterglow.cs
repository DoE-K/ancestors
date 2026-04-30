using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Simulates moonlight reflection on water surfaces.
/// Fades a Point Light 2D in at night and out during the day.
/// Adds a subtle shimmer using Perlin noise.
/// </summary>
public class WaterGlow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light2D  _light;
    [SerializeField] private DayNight _dayNight;

    [Header("Night Settings")]
    [SerializeField] private float _nightIntensity = 0.6f;
    [SerializeField] private float _nightRadius    = 5f;
    [SerializeField] private Color _moonColor      = new Color(0.6f, 0.75f, 1.0f);

    [Header("Shimmer")]
    [SerializeField] private float _shimmerAmount = 0.08f;
    [SerializeField] private float _shimmerSpeed  = 1.2f;

    [Header("Transition Speed")]
    [SerializeField] private float _fadeSpeed = 1.5f;

    private float _seed;
    private float _currentIntensity = 0f;

    private void Awake()
    {
        _seed = Random.Range(0f, 100f);
        if (_light != null) _light.color = _moonColor;
    }

    private void Update()
    {
        if (_light == null || _dayNight == null) return;

        float targetIntensity = _dayNight.IsNight ? _nightIntensity : 0f;

        _currentIntensity = Mathf.Lerp(
            _currentIntensity, targetIntensity, _fadeSpeed * Time.deltaTime);

        // Shimmer
        float shimmer = Mathf.PerlinNoise(Time.time * _shimmerSpeed, _seed) - 0.5f;
        _light.intensity             = _currentIntensity + shimmer * _shimmerAmount;
        _light.pointLightOuterRadius = _nightRadius;
    }
}