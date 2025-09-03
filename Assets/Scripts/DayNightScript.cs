using UnityEngine;
using TMPro;

public class DayNightScript : MonoBehaviour
{
    public float dayDuration = 60f; // Dauer eines vollen Zyklus (Tag ? Nacht ? Tag)

    // Farbe des Overlays (schwarz f�r Dunkelheit)
    public Color overlayColor = Color.black;

    // Transparenz f�r Nacht und Tag
    public float nightAlpha = 0.6f; // Wie dunkel es nachts wird (0.6 = 60% dunkel)
    public float dayAlpha = 0f;     // Wie hell es tags ist (0 = komplett durchsichtig)

    private float time;
    public SpriteRenderer globalTintOverlay;

    public TextMeshProUGUI clockText;

    void Update()
    {
        time += Time.deltaTime;

        // t geht von 0 -> 1 -> 0 ? weicher �bergang Tag/Nacht
        float t = Mathf.PingPong(time / dayDuration, 1f);

        // Lerp zwischen Nacht und Tag-Transparenz
        float currentAlpha = Mathf.Lerp(nightAlpha, dayAlpha, t);

        // Setze neue Overlay-Farbe mit Alpha
        globalTintOverlay.color = new Color(overlayColor.r, overlayColor.g, overlayColor.b, currentAlpha);



        // Holt aktuelle Uhrzeit vom System
        System.DateTime currentTime = System.DateTime.Now;

        // Formatiert als HH:mm:ss (z. B. 13:45:09)
        string timeString = currentTime.ToString("HH:mm:ss");

        // Uhrzeit anzeigen
        clockText.text = "Uhrzeit: " + timeString;
    }
}
