using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public enum ZoomLevel { Far, Normal, Close }

    [Header("Zoom Sizes")]
    [SerializeField] private float _farSize    = 8f;
    [SerializeField] private float _normalSize = 5f;
    [SerializeField] private float _closeSize  = 3.5f;

    [Header("Transition")]
    [SerializeField] private float _speed = 1.5f;

    private Camera _camera;
    private float  _targetSize;

    private void Awake()
    {
        _camera     = GetComponent<Camera>();
        _targetSize = _camera.orthographicSize;
    }

    private void Update()
    {
        _camera.orthographicSize = Mathf.Lerp(
            _camera.orthographicSize, _targetSize, _speed * Time.deltaTime);
    }

    /// <summary>Transition to a named zoom level.</summary>
    public void SetTarget(ZoomLevel level)
    {
        _targetSize = level switch
        {
            ZoomLevel.Far    => _farSize,
            ZoomLevel.Normal => _normalSize,
            ZoomLevel.Close  => _closeSize,
            _                => _normalSize
        };
    }

    /// <summary>
    /// Transition to any arbitrary size.
    /// Used by tool effects like TelescopeEffect.
    /// </summary>
    public void SetCustomSize(float size)
    {
        _targetSize = Mathf.Max(0.1f, size);
    }
}