using UnityEngine;
using TMPro;

public class DayNightScript : MonoBehaviour
{
    [Tooltip("Sekunden pro voller Spieltag (z.B. 1200 = 20 Minuten real = 24h ingame)")]
    public float dayDuration = 1200f; // Sekunden pro voller Spiel-24h

    public Color overlayColor = Color.black;
    [Range(0f,1f)] public float nightAlpha = 0.8f;
    [Range(0f,1f)] public float dayAlpha = 0f;

    // Sunrise/Sunset Einstellungen (in ingame Stunden)
    public float sunriseHour = 6f;
    public float sunsetHour = 18f;
    public float transitionHours = 1f; // Dauer des Übergangs (z.B. 1 Stunde)

    private float elapsedTime = 0f;
    public SpriteRenderer globalTintOverlay;

    public TextMeshProUGUI clockText;
    public TextMeshProUGUI temperatureText;

    void Update()
    {
        if (dayDuration <= 0f) return; // Sicherheitscheck

        elapsedTime += Time.deltaTime;

        // Normierte Tageszeit 0..1
        float normalizedTime = (elapsedTime % dayDuration) / dayDuration;

        // Spielzeit in Stunden 0..24
        float gameHours = normalizedTime * 24f;
        int hours = Mathf.FloorToInt(gameHours) % 24;
        int minutes = Mathf.FloorToInt((gameHours - Mathf.Floor(gameHours)) * 60f);

        if (clockText != null)
            clockText.text = $"Time: {hours:D2}:{minutes:D2}";

        // Berechne Alpha mit weichem Übergang um sunrise/sunset
        float alpha = ComputeDayNightAlpha(gameHours);
        if (globalTintOverlay != null)
            globalTintOverlay.color = new Color(overlayColor.r, overlayColor.g, overlayColor.b, alpha);

        // Temperaturbeispiel
        int temperature = (hours >= 6 && hours < 18) ? 25 : 10;
        if (temperatureText != null)
            temperatureText.text = $"Temp: {temperature}°C";
    }

    private float ComputeDayNightAlpha(float hour)
    {
        // hour in [0,24)
        hour = (hour + 24f) % 24f;

        float startSunrise = sunriseHour - transitionHours;
        float endSunrise   = sunriseHour + transitionHours;
        float startSunset  = sunsetHour - transitionHours;
        float endSunset    = sunsetHour + transitionHours;

        // Hilfs-Lerps für Übergänge
        if (hour < startSunrise) return nightAlpha;
        if (hour < endSunrise)
            return Mathf.Lerp(nightAlpha, dayAlpha, Mathf.InverseLerp(startSunrise, endSunrise, hour));
        if (hour < startSunset) return dayAlpha;
        if (hour < endSunset)
            return Mathf.Lerp(dayAlpha, nightAlpha, Mathf.InverseLerp(startSunset, endSunset, hour));
        return nightAlpha;
    }
}
