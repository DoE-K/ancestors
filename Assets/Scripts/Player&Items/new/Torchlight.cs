using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Simulates a realistic flickering torch using Perlin noise on a Point Light 2D.
/// Attach to the Fackel/Torch GameObject alongside a Light2D component.
/// </summary>
public class TorchLight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light2D _light;

    [Header("Base Values")]
    [SerializeField] private float _baseIntensity   = 1.2f;
    [SerializeField] private float _baseRadius      = 4f;

    [Header("Flicker — Intensity")]
    [SerializeField] private float _intensityFlicker  = 0.3f;
    [SerializeField] private float _intensitySpeed    = 3f;

    [Header("Flicker — Radius")]
    [SerializeField] private float _radiusFlicker     = 0.4f;
    [SerializeField] private float _radiusSpeed       = 2f;

    [Header("Color")]
    [SerializeField] private Color _torchColor = new Color(1f, 0.6f, 0.2f);

    private float _seed;

    private void Awake()
    {
        _seed = Random.Range(0f, 100f);
        if (_light != null) _light.color = _torchColor;
    }

    private void Update()
    {
        if (_light == null) return;

        float t = Time.time + _seed;

        // Perlin noise gives organic, non-repeating flicker
        float intensityNoise = Mathf.PerlinNoise(t * _intensitySpeed, _seed);
        float radiusNoise    = Mathf.PerlinNoise(_seed, t * _radiusSpeed);

        _light.intensity             = _baseIntensity
            + (intensityNoise - 0.5f) * _intensityFlicker;

        _light.pointLightOuterRadius = _baseRadius
            + (radiusNoise - 0.5f) * _radiusFlicker;
    }
}