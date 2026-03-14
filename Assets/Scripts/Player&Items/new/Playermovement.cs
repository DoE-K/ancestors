using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float _normalSpeed = 5f;
    [SerializeField] private float _sneakSpeed  = 1f;

    [Header("References")]
    [SerializeField] private GameObject _sprite;

    private Rigidbody2D _rb;
    private Vector2 _movement;
    private float _currentSpeed;
    private bool _isFacingRight = true;

    // ── Unity lifecycle ──────────────────────────────────────────────────────

    private void Start()
    {
        _rb           = GetComponent<Rigidbody2D>();
        _currentSpeed = _normalSpeed;
    }

    private void Update()
    {
        ReadMovementInput();
        UpdateSpeed();
        UpdateFacing();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * _currentSpeed * Time.fixedDeltaTime);
    }

    // ── Private methods ──────────────────────────────────────────────────────

    private void ReadMovementInput()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
    }

    private void UpdateSpeed()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) _currentSpeed = _sneakSpeed;
        if (Input.GetKeyUp(KeyCode.LeftShift))   _currentSpeed = _normalSpeed;
    }

    private void UpdateFacing()
    {
        if (_movement.x < 0 && _isFacingRight)  Flip();
        if (_movement.x > 0 && !_isFacingRight) Flip();
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;

        var scale = _sprite.transform.localScale;
        scale.x *= -1;
        _sprite.transform.localScale = scale;
    }
}