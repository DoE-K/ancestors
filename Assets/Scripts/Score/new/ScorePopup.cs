using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float _moveSpeed = 40f;
    [SerializeField] private float _fadeTime  = 0.8f;

    [Header("References")]
    [SerializeField] private GameObject        _popupObject;
    [SerializeField] private TextMeshProUGUI   _popupText;

    private Color         _originalColor;
    private RectTransform _rectTransform;
    private Vector2       _originalPosition;
    private bool          _isAnimating = false;

    // ── Unity lifecycle ──────────────────────────────────────────────────────

    private void Start()
    {
        _originalColor    = _popupText.color;
        _rectTransform    = _popupText.GetComponent<RectTransform>();
        _originalPosition = _rectTransform.anchoredPosition;

        _popupObject.SetActive(false);
    }

    private void Update()
    {
        if (!_isAnimating) return;

        // Float upward
        _popupText.transform.Translate(Vector3.up * _moveSpeed * Time.deltaTime);

        // Fade out
        float newAlpha = _popupText.color.a - Time.deltaTime / _fadeTime;
        _popupText.color = new Color(
            _originalColor.r,
            _originalColor.g,
            _originalColor.b,
            newAlpha
        );

        // Animation complete — reset
        if (newAlpha <= 0f)
        {
            Reset();
        }
    }

    // ── Public API ───────────────────────────────────────────────────────────

    public void Show(int points)
    {
        // Reset state before starting a new animation
        Reset();

        _popupText.text  = "+" + points;
        _popupObject.SetActive(true);
        _isAnimating = true;
    }

    // ── Private helpers ──────────────────────────────────────────────────────

    private void Reset()
    {
        _isAnimating = false;
        _rectTransform.anchoredPosition = _originalPosition;
        _popupText.color = _originalColor;
        _popupObject.SetActive(false);
    }
}