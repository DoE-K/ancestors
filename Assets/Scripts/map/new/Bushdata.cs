using System.Collections;
using UnityEngine;

public class BerryGenerator : MonoBehaviour
{
    [Header("Bush Type")]
    [Tooltip("Assign the BushData ScriptableObject for this bush (defines which berry grows here).")]
    [SerializeField] private BushData _bushData;

    [Header("Spawn Points")]
    [Tooltip("Assign the BerrySpawnPoint component on this bush.")]
    [SerializeField] private BerrySpawnPoint _berrySpawnPoint;

    [Header("Growth Settings")]
    [SerializeField] private float   _growDuration = 2f;
    [SerializeField] private Vector3 _finalScale   = Vector3.one;

    [Header("Respawn Delay")]
    [SerializeField] private float _minGrowDelay = 1f;
    [SerializeField] private float _maxGrowDelay = 5f;

    // ── Unity lifecycle ──────────────────────────────────────────────────────

    private void Start()
    {
        if (_bushData == null)
        {
            Debug.LogError($"[BerryGenerator] '{name}' has no BushData assigned!", this);
            return;
        }

        if (_berrySpawnPoint == null)
        {
            Debug.LogError($"[BerryGenerator] '{name}' has no BerrySpawnPoint assigned!", this);
            return;
        }

        foreach (var spawnPoint in _berrySpawnPoint.spawnPoints)
        {
            StartCoroutine(BerryLifecycle(spawnPoint));
        }
    }

    // ── Coroutines ───────────────────────────────────────────────────────────

    private IEnumerator BerryLifecycle(Transform spawnPoint)
    {
        while (true)
        {
            // Wait until the slot is empty (berry was picked up or never existed)
            yield return new WaitUntil(() => spawnPoint.childCount == 0);

            // Random delay before regrowing
            float delay = Random.Range(_minGrowDelay, _maxGrowDelay);
            yield return new WaitForSeconds(delay);

            // Spawn and grow the berry
            var berry = Instantiate(
                _bushData.berryPrefab,
                spawnPoint.position,
                Quaternion.identity,
                spawnPoint
            );

            berry.transform.localScale = Vector3.zero;
            yield return StartCoroutine(GrowBerry(berry));

            // Wait until this berry is removed before cycling again
            yield return new WaitUntil(() => spawnPoint.childCount == 0);
        }
    }

    private IEnumerator GrowBerry(GameObject berry)
    {
        float elapsed = 0f;

        while (elapsed < _growDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / _growDuration);
            berry.transform.localScale = Vector3.Lerp(Vector3.zero, _finalScale, progress);
            yield return null;
        }

        berry.transform.localScale = _finalScale;
    }
}