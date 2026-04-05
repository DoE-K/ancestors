using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer _lightSprite;

    [Header("Night Settings")]
    [Tooltip("Scale of the light circle at full intensity.")]
    [SerializeField] private float _targetScale     = 6f;
    [Tooltip("Alpha of the sprite at full intensity (0–1).")]
    [SerializeField] private float _targetAlpha     = 0.85f;

    [Header("Transition")]
    [SerializeField] private float _transitionSpeed = 2f;

    private bool  _active          = false;
    private float _currentAlpha    = 0f;
    private float _currentScale    = 0f;

    private void Update()
    {
        float targetAlpha = _active ? _targetAlpha : 0f;
        float targetScale = _active ? _targetScale : 0f;

        _currentAlpha = Mathf.Lerp(_currentAlpha, targetAlpha, _transitionSpeed * Time.deltaTime);
        _currentScale = Mathf.Lerp(_currentScale, targetScale, _transitionSpeed * Time.deltaTime);

        if (_lightSprite == null) return;

        var color = _lightSprite.color;
        color.a = _currentAlpha;
        _lightSprite.color = color;

        _lightSprite.transform.localScale = Vector3.one * _currentScale;
    }

    public void SetActive(bool active)
    {
        _active = active;
    }
}