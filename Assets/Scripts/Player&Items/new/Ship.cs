using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Ship : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed    = 2f;
    [SerializeField] private float _sailDuration = 10f;

    [Header("References")]
    [SerializeField] private Transform _shipCenter;
    [SerializeField] private Canvas    _mapUI;

    [Header("Scene")]
    [Tooltip("Exact name of the game over / highscore scene.")]
    [SerializeField] private string _gameOverSceneName = "GameOver";

    [Header("Input")]
    [SerializeField] private KeyCode _boardKey = KeyCode.E;

    private Transform        _playerTransform;
    private PlayerMovement   _playerMovement;
    private Collider2D       _playerCollider;
    private bool             _playerNearby       = false;
    private bool             _playerOnBoard      = false;
    private bool             _sceneChangeStarted = false;

    // ── Unity lifecycle ───────────────────────────────────────────────────────

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
        _playerCollider  = other;
        _playerMovement  = other.GetComponent<PlayerMovement>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _playerNearby = false;
    }

    // ── Private ───────────────────────────────────────────────────────────────

    private void BoardShip()
    {
        _playerOnBoard            = true;
        _playerTransform.position = _shipCenter.position;

        // Disable player control
        if (_playerMovement  != null) _playerMovement.enabled  = false;
        if (_playerCollider  != null) _playerCollider.enabled  = false;
        if (_mapUI           != null) _mapUI.enabled            = false;

        if (!_sceneChangeStarted)
        {
            _sceneChangeStarted = true;
            StartCoroutine(SailAndLoad());
        }
    }

    private void MoveShip()
    {
        var delta = Vector2.left * _moveSpeed * Time.deltaTime;
        transform.Translate(delta);
        if (_playerTransform != null)
            _playerTransform.Translate(delta);
    }

    private IEnumerator SailAndLoad()
    {
        yield return new WaitForSeconds(_sailDuration);

        // Save the final score before scene transition
        var player = FindAnyObjectByType<Player>();
        player?.SubmitHighscore();

        SceneManager.LoadScene(_gameOverSceneName);
    }
}