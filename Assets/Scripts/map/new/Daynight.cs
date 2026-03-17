using UnityEngine;
using TMPro;

public class DayNight : MonoBehaviour
{
    [Header("Time Settings")]
    [Tooltip("Real seconds for one full in-game day. 1200 = 20 min real = 24h in-game.")]
    [SerializeField] private float _dayDuration = 1200f;

    [Header("Day/Night Transition")]
    [SerializeField] private Color  _overlayColor     = Color.black;
    [SerializeField][Range(0f, 1f)] private float _nightAlpha       = 0.8f;
    [SerializeField][Range(0f, 1f)] private float _dayAlpha         = 0f;
    [SerializeField] private float  _sunriseHour      = 6f;
    [SerializeField] private float  _sunsetHour       = 18f;
    [SerializeField] private float  _transitionHours  = 1f;

    [Header("Temperature")]
    [SerializeField] private int _dayTemperature   = 25;
    [SerializeField] private int _nightTemperature = 10;

    [Header("References")]
    [SerializeField] private SpriteRenderer    _globalTintOverlay;
    [SerializeField] private TextMeshProUGUI   _clockText;
    [SerializeField] private TextMeshProUGUI   _temperatureText;

    private float _elapsedTime = 0f;

    // ── Unity lifecycle ──────────────────────────────────────────────────────

    private void Update()
    {
        if (_dayDuration <= 0f) return;

        _elapsedTime += Time.deltaTime;

        float gameHours = GetCurrentGameHour();

        UpdateClock(gameHours);
        UpdateOverlay(gameHours);
        UpdateTemperature(gameHours);
    }

    // ── Private methods ──────────────────────────────────────────────────────

    private float GetCurrentGameHour()
    {
        float normalizedTime = (_elapsedTime % _dayDuration) / _dayDuration;
        return normalizedTime * 24f;
    }

    private void UpdateClock(float gameHours)
    {
        if (_clockText == null) return;

        int hours   = Mathf.FloorToInt(gameHours) % 24;
        int minutes = Mathf.FloorToInt((gameHours - Mathf.Floor(gameHours)) * 60f);
        _clockText.text = $"Time: {hours:D2}:{minutes:D2}";
    }

    private void UpdateOverlay(float gameHours)
    {
        if (_globalTintOverlay == null) return;

        float alpha = ComputeDayNightAlpha(gameHours);
        _globalTintOverlay.color = new Color(
            _overlayColor.r,
            _overlayColor.g,
            _overlayColor.b,
            alpha
        );
    }

    private void UpdateTemperature(float gameHours)
    {
        if (_temperatureText == null) return;

        bool isDaytime = gameHours >= _sunriseHour && gameHours < _sunsetHour;
        int temp = isDaytime ? _dayTemperature : _nightTemperature;
        _temperatureText.text = $"Temp: {temp}°C";
    }

    private float ComputeDayNightAlpha(float hour)
    {
        hour = (hour + 24f) % 24f;

        float startSunrise = _sunriseHour - _transitionHours;
        float endSunrise   = _sunriseHour + _transitionHours;
        float startSunset  = _sunsetHour  - _transitionHours;
        float endSunset    = _sunsetHour  + _transitionHours;

        if (hour < startSunrise) return _nightAlpha;
        if (hour < endSunrise)
            return Mathf.Lerp(_nightAlpha, _dayAlpha,
                Mathf.InverseLerp(startSunrise, endSunrise, hour));
        if (hour < startSunset) return _dayAlpha;
        if (hour < endSunset)
            return Mathf.Lerp(_dayAlpha, _nightAlpha,
                Mathf.InverseLerp(startSunset, endSunset, hour));

        return _nightAlpha;
    }
}