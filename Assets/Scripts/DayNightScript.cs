using UnityEngine;
using TMPro;

public class DayNightScript : MonoBehaviour
{
    public float dayDuration = 60f; 

    
    public Color overlayColor = Color.black;

    
    public float nightAlpha = 0.6f; 
    public float dayAlpha = 0f;     

    private float time;
    public SpriteRenderer globalTintOverlay;

    public TextMeshProUGUI clockText;

    void Update()
    {
        time += Time.deltaTime;

        
        float t = Mathf.PingPong(time / dayDuration, 1f);

        
        float currentAlpha = Mathf.Lerp(nightAlpha, dayAlpha, t);

        
        globalTintOverlay.color = new Color(overlayColor.r, overlayColor.g, overlayColor.b, currentAlpha);



        
        System.DateTime currentTime = System.DateTime.Now;

        
        string timeString = currentTime.ToString("HH:mm:ss");

        
        clockText.text = "Uhrzeit: " + timeString;
    }
}
