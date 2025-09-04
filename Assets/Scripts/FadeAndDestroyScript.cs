using UnityEngine;

public class FadeAndDestroy : MonoBehaviour
{
    public float fadeDuration = 3f; 

    private SpriteRenderer sr;
    private Color originalColor;
    private float timer = 1f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float alpha = Mathf.Lerp(originalColor.a, 0f, timer / fadeDuration);
        sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

        if (timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}
