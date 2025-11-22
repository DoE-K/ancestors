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
        pointPopup.SetActive(false);
    }

    void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        float newAlpha = text.color.a - Time.deltaTime / fadeTime;
        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

        if (newAlpha <= 0f)
        {
            rectTransform.anchoredPosition = originalPos;
            text.color = originalColor;
            pointPopup.SetActive(false);
        }
    }
}
