using UnityEngine;
using TMPro;

public class DayNight : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float _dayDuration = 1200f;

    [Header("Day/Night Transition")]
    [SerializeField] private Color  _overlayColor    = Color.black;
    [SerializeField][Range(0f, 1f)] private float _nightAlpha     = 0.8f;
    [SerializeField][Range(0f, 1f)] private float _dayAlpha       = 0f;
    [SerializeField] private float  _sunriseHour     = 6f;
    [SerializeField] private float  _sunsetHour      = 18f;
    [SerializeField] private float  _transitionHours = 1f;

    [Header("Temperature")]
    [SerializeField] private float _baseMaxTemp      = 25f;
    [SerializeField] private float _baseMinTemp      = 10f;
    [SerializeField] private float _dailyMaxVariation = 4f;
    [SerializeField] private float _dailyMinVariation = 3f;
    [SerializeField] private float _peakHour         = 14f;

    [Header("References")]
    [SerializeField] private SpriteRenderer  _globalTintOverlay;
    [SerializeField] private TextMeshProUGUI _clockText;
    [SerializeField] private TextMeshProUGUI _temperatureText;

    // ── Public state ──────────────────────────────────────────────────────────
    public bool  IsNight        { get; private set; }
    public float Temperature    { get; private set; }
    public float CurrentHour    { get; private set; }  // ← NEW: used by DayNightLighting

    // ── Private state ─────────────────────────────────────────────────────────
    private float _elapsedTime   = 0f;
    private float _todayMaxTemp;
    private float _todayMinTemp;
    private int   _lastDay       = -1;

    private void Start() => RollDailyTemperatures();

    private void Update()
    {
        if (_dayDuration <= 0f) return;

        _elapsedTime += Time.deltaTime;

        int currentDay = Mathf.FloorToInt(_elapsedTime / _dayDuration);
        if (currentDay != _lastDay) { _lastDay = currentDay; RollDailyTemperatures(); }

        CurrentHour = GetCurrentGameHour();
        IsNight     = CurrentHour < _sunriseHour || CurrentHour >= _sunsetHour;
        Temperature = ComputeTemperature(CurrentHour);

        UpdateClock(CurrentHour);
        UpdateOverlay(CurrentHour);
        UpdateTemperatureText();
    }

    private float GetCurrentGameHour()
    {
        return (_elapsedTime % _dayDuration) / _dayDuration * 24f;
    }

    private void RollDailyTemperatures()
    {
        _todayMaxTemp = _baseMaxTemp + Random.Range(-_dailyMaxVariation, _dailyMaxVariation);
        _todayMinTemp = _baseMinTemp + Random.Range(-_dailyMinVariation, _dailyMinVariation);
        _todayMinTemp = Mathf.Min(_todayMinTemp, _todayMaxTemp - 1f);
    }

    private float ComputeTemperature(float hour)
    {
        float hoursFromPeak = Mathf.Abs(hour - _peakHour);
        if (hoursFromPeak > 12f) hoursFromPeak = 24f - hoursFromPeak;
        float t = (1f - Mathf.Cos(hoursFromPeak / 12f * Mathf.PI)) / 2f;
        return Mathf.Lerp(_todayMaxTemp, _todayMinTemp, t);
    }

    private void UpdateClock(float gameHours)
    {
        if (_clockText == null) return;
        int hours   = Mathf.FloorToInt(gameHours) % 24;
        int minutes = Mathf.FloorToInt((gameHours - Mathf.Floor(gameHours)) * 60f);
        _clockText.text = $"{hours:D2}:{minutes:D2}";
    }

    private void UpdateOverlay(float gameHours)
    {
        if (_globalTintOverlay == null) return;
        float alpha = ComputeDayNightAlpha(gameHours);
        _globalTintOverlay.color = new Color(
            _overlayColor.r, _overlayColor.g, _overlayColor.b, alpha);
    }

    private void UpdateTemperatureText()
    {
        if (_temperatureText == null) return;
        _temperatureText.text = $"{Temperature:F1}°C";
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