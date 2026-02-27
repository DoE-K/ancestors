using UnityEngine;
using TMPro;

public class DayNightScript : MonoBehaviour
{
    [Tooltip("e.g. 1200 = 20 minutes real = 24h ingame)")]
    public float dayDuration = 1200f;

    public Color overlayColor = Color.black;
    [Range(0f,1f)] public float nightAlpha = 0.8f;
    [Range(0f,1f)] public float dayAlpha = 0f;

    public float sunriseHour = 6f;
    public float sunsetHour = 18f;
    public float transitionHours = 1f;

    private float elapsedTime = 0f;
    public SpriteRenderer globalTintOverlay;

    public TextMeshProUGUI clockText;
    public TextMeshProUGUI temperatureText;

    void Update()
    {
        if (dayDuration <= 0f) return;

        elapsedTime += Time.deltaTime;

        float normalizedTime = (elapsedTime % dayDuration) / dayDuration;

        float gameHours = normalizedTime * 24f;
        int hours = Mathf.FloorToInt(gameHours) % 24;
        int minutes = Mathf.FloorToInt((gameHours - Mathf.Floor(gameHours)) * 60f);

        if (clockText != null)
            clockText.text = $"Time: {hours:D2}:{minutes:D2}";

        float alpha = ComputeDayNightAlpha(gameHours);
        if (globalTintOverlay != null)
            globalTintOverlay.color = new Color(overlayColor.r, overlayColor.g, overlayColor.b, alpha);

        int temperature = (hours >= 6 && hours < 18) ? 25 : 10;
        if (temperatureText != null)
            temperatureText.text = $"Temp: {temperature}°C";
    }

    private float ComputeDayNightAlpha(float hour)
    {
        hour = (hour + 24f) % 24f;

        float startSunrise = sunriseHour - transitionHours;
        float endSunrise   = sunriseHour + transitionHours;
        float startSunset  = sunsetHour - transitionHours;
        float endSunset    = sunsetHour + transitionHours;

        if (hour < startSunrise) return nightAlpha;
        if (hour < endSunrise)
            return Mathf.Lerp(nightAlpha, dayAlpha, Mathf.InverseLerp(startSunrise, endSunrise, hour));
        if (hour < startSunset) return dayAlpha;
        if (hour < endSunset)
            return Mathf.Lerp(dayAlpha, nightAlpha, Mathf.InverseLerp(startSunset, endSunset, hour));
        return nightAlpha;
    }
}
