using UnityEngine;
using TMPro;

public class PointsPopUp : MonoBehaviour
{
    public float moveSpeed = 40f; 
    public float lifetime = 1f; 
    public float fadeTime = 0.8f; 
    public GameObject pointPopup; 
    private TextMeshProUGUI text; 
    private Color originalColor; 

    private RectTransform rectTransform;
    private Vector2 originalPos;
    
    void Start() 
    { 
        text = GetComponent<TextMeshProUGUI>(); 
        originalColor = text.color; 

        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.anchoredPosition;
        pointPopup.SetActive(false); //Destroy(gameObject, lifetime); 
    }

    void Update()
    {
        
        // Nach oben bewegen
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Fade Timer runterzählen
        //fadeTimer -= Time.deltaTime;

        // Neuen Alpha Wert berechnen
        float newAlpha = text.color.a - Time.deltaTime / fadeTime;
        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

        // Wenn komplett unsichtbar → deaktivieren
        if (newAlpha <= 0f)
        {
            //transform.position = new Vector3 (245, 135, 0);
            rectTransform.anchoredPosition = originalPos;
            text.color = originalColor;
            pointPopup.SetActive(false); // oder gameObject.SetActive(false);
        }
    }
}
