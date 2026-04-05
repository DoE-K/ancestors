using UnityEngine;
using UnityEngine.UI;

public class VignetteController : MonoBehaviour
{
    public enum VignetteMode { Soft, Dark, Pulse }

    [Header("References")]
    [SerializeField] private Image _vignetteImage;

    [Header("Alpha Values")]
    [SerializeField] private float _softAlpha  = 0.25f;
    [SerializeField] private float _darkAlpha  = 0.65f;
    [SerializeField] private float _pulseMin   = 0.55f;
    [SerializeField] private float _pulseMax   = 0.80f;
    [SerializeField] private float _pulseSpeed = 2f;

    [Header("Transition")]
    [SerializeField] private float _transitionSpeed = 2f;

    private VignetteMode _mode       = VignetteMode.Soft;
    private float        _targetAlpha;
    private float        _pulseTimer = 0f;

    private void Start()
    {
        _targetAlpha = _softAlpha;
    }

    private void Update()
    {
        if (_mode == VignetteMode.Pulse)
        {
            _pulseTimer += Time.deltaTime * _pulseSpeed;
            _targetAlpha = Mathf.Lerp(_pulseMin, _pulseMax,
                (Mathf.Sin(_pulseTimer) + 1f) / 2f);
        }

        var color = _vignetteImage.color;
        color.a = Mathf.Lerp(color.a, _targetAlpha, _transitionSpeed * Time.deltaTime);
        _vignetteImage.color = color;
    }

    public void SetMode(VignetteMode mode)
    {
        _mode       = mode;
        _pulseTimer = 0f;

        _targetAlpha = mode switch
        {
            VignetteMode.Soft  => _softAlpha,
            VignetteMode.Dark  => _darkAlpha,
            VignetteMode.Pulse => _pulseMin,
            _                  => _softAlpha
        };
    }
}