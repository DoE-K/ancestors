/*using UnityEngine;

public class AtmosphereManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private DayNight           _dayNight;
    [SerializeField] private AnimalSensor       _playerSensor;   // optional — detects danger
    [SerializeField] private CameraZoom         _cameraZoom;
    [SerializeField] private VignetteController _vignette;
    [SerializeField] private PlayerLight        _playerLight;

    [Header("Transition Speed")]
    [Tooltip("How fast atmosphere transitions between states (lower = slower).")]
    [SerializeField] private float _transitionSpeed = 1.5f;

    // ── Private state ─────────────────────────────────────────────────────────
    private AtmosphereState _currentState = AtmosphereState.Day;
    private AtmosphereState _targetState  = AtmosphereState.Day;

    public enum AtmosphereState { Day, Night, Danger }

    // ── Unity lifecycle ───────────────────────────────────────────────────────

    private void Update()
    {
        _targetState = EvaluateState();

        if (_targetState != _currentState)
        {
            _currentState = _targetState;
            ApplyState(_currentState);
        }
    }

    // ── Private methods ───────────────────────────────────────────────────────

    private AtmosphereState EvaluateState()
    {
        // Danger overrides everything
        if (_playerSensor != null && _playerSensor.CanSeePredator)
            return AtmosphereState.Danger;

        if (_dayNight != null && _dayNight.IsNight)
            return AtmosphereState.Night;

        return AtmosphereState.Day;
    }

    private void ApplyState(AtmosphereState state)
    {
        switch (state)
        {
            case AtmosphereState.Day:
                _cameraZoom?.SetTarget(CameraZoom.ZoomLevel.Far);
                _vignette?.SetMode(VignetteController.VignetteMode.Soft);
                _playerLight?.SetActive(false);
                break;

            case AtmosphereState.Night:
                _cameraZoom?.SetTarget(CameraZoom.ZoomLevel.Normal);
                _vignette?.SetMode(VignetteController.VignetteMode.Dark);
                _playerLight?.SetActive(true);
                break;

            case AtmosphereState.Danger:
                _cameraZoom?.SetTarget(CameraZoom.ZoomLevel.Close);
                _vignette?.SetMode(VignetteController.VignetteMode.Pulse);
                _playerLight?.SetActive(_dayNight?.IsNight ?? false);
                break;
        }
    }
}*/