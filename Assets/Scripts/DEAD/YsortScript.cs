/*using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSort : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // Sortierung basierend auf Y-Position (mit Multiplikator für Genauigkeit)
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}*/
