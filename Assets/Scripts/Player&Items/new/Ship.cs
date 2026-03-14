using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Ship : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed   = 2f;
    [SerializeField] private float _sailDuration = 15f;

    [Header("References")]
    [SerializeField] private Transform _shipCenter;
    [SerializeField] private Canvas    _mapUI;

    [Header("Input")]
    [SerializeField] private KeyCode _boardKey = KeyCode.E;

    private Transform   _playerTransform;
    private Collider2D  _playerCollider;
    private bool        _playerNearby      = false;
    private bool        _playerOnBoard     = false;
    private bool        _sceneChangeStarted = false;

    // ── Unity lifecycle ──────────────────────────────────────────────────────

    private void Update()
    {
        if (_playerNearby && !_playerOnBoard && Input.GetKeyDown(_boardKey))
            BoardShip();

        if (_playerOnBoard)
            MoveShip();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _playerNearby    = true;
        _playerTransform = other.transform;
        _playerCollider  = other;          // ← assigned here, not in Start()
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _playerNearby = false;
    }

    // ── Private methods ──────────────────────────────────────────────────────

    private void BoardShip()
    {
        _playerTransform.position = _shipCenter.position;
        _playerOnBoard            = true;

        // Disable once — not every frame
        if (_playerCollider != null) _playerCollider.enabled = false;
        if (_mapUI != null)          _mapUI.enabled           = false;

        if (!_sceneChangeStarted)
        {
            _sceneChangeStarted = true;
            StartCoroutine(ChangeSceneAfterDelay(_sailDuration));
        }
    }

    private void MoveShip()
    {
        var delta = Vector2.left * _moveSpeed * Time.deltaTime;
        transform.Translate(delta);
        _playerTransform.Translate(delta);
    }

    private IEnumerator ChangeSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}