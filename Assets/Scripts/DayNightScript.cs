using UnityEngine;
using TMPro;

public class DayNightScript : MonoBehaviour
{
    public float dayDuration = 1200f; // 20 Minuten = 1200 Sekunden für 24h
    public Color overlayColor = Color.black;

    public float nightAlpha = 0.6f;
    public float dayAlpha = 0f;

    private float time; // interne Zeit in Sekunden
    public SpriteRenderer globalTintOverlay;

    public TextMeshProUGUI clockText;
    public TextMeshProUGUI temperatureText; // neue Temperaturanzeige

    void Update()
    {
        // Zeit fortschreiten lassen
        time += Time.deltaTime;

        // Modulo, damit die Zeit nicht unendlich wächst
        float normalizedTime = (time % dayDuration) / dayDuration;

        // Berechne die Spielzeit in Stunden (0–24h)
        float gameHours = normalizedTime * 24f;
        int hours = Mathf.FloorToInt(gameHours);
        int minutes = Mathf.FloorToInt((gameHours - hours) * 60f);
        int seconds = Mathf.FloorToInt(((gameHours - hours) * 60f - minutes) * 60f);

        // === Overlay berechnen (Helligkeit) ===
        // PingPong sorgt für Hell/Dunkel Übergänge
        float t = Mathf.PingPong(normalizedTime * 2f, 1f);
        float currentAlpha = Mathf.Lerp(nightAlpha, dayAlpha, t);

        globalTintOverlay.color = new Color(overlayColor.r, overlayColor.g, overlayColor.b, currentAlpha);

        // === Uhrzeit-Text ===
        string timeString = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        clockText.text = "Time: " + timeString;

        // === Temperatur bestimmen ===
        int temperature;
        if (hours >= 6 && hours < 18) // Tag
            temperature = 25;
        else // Nacht
            temperature = 10;

        temperatureText.text = "Temp: " + temperature + "°C";
    }
}
