using System.Collections;
using UnityEngine;

public class FruitGenerator : MonoBehaviour
{
    [Header("Source Type")]
    [Tooltip("Assign the FruitData ScriptableObject (defines which fruit grows here).")]
    [SerializeField] private FruitData _fruitData;

    [Header("Spawn Points")]
    [Tooltip("Assign the FruitSpawnPoint component on this bush or tree.")]
    [SerializeField] private FruitSpawnPoint _fruitSpawnPoint;

    [Header("Growth Settings")]
    [SerializeField] private float   _growDuration = 2f;
    [SerializeField] private Vector3 _finalScale   = Vector3.one;

    [Header("Respawn Delay")]
    [SerializeField] private float _minGrowDelay = 1f;
    [SerializeField] private float _maxGrowDelay = 5f;

    // ── Unity lifecycle ──────────────────────────────────────────────────────

    private void Start()
    {
        if (_fruitData == null)
        {
            Debug.LogError($"[FruitGenerator] '{name}' has no FruitData assigned!", this);
            return;
        }

        if (_fruitSpawnPoint == null)
        {
            Debug.LogError($"[FruitGenerator] '{name}' has no FruitSpawnPoint assigned!", this);
            return;
        }

        foreach (var spawnPoint in _fruitSpawnPoint.spawnPoints)
            StartCoroutine(FruitLifecycle(spawnPoint));
    }

    private IEnumerator FruitLifecycle(Transform spawnPoint)
    {
        while (true)
        {
            yield return new WaitUntil(() => spawnPoint.childCount == 0);

            float delay = Random.Range(_minGrowDelay, _maxGrowDelay);
            yield return new WaitForSeconds(delay);

            var fruit = Instantiate(
                _fruitData.fruitPrefab,
                spawnPoint.position,
                Quaternion.identity,
                spawnPoint
            );

            fruit.transform.localScale = Vector3.zero;
            yield return StartCoroutine(GrowFruit(fruit));

            yield return new WaitUntil(() => spawnPoint.childCount == 0);
        }
    }

    private IEnumerator GrowFruit(GameObject fruit)
    {
        float elapsed = 0f;

        while (elapsed < _growDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / _growDuration);
            fruit.transform.localScale = Vector3.Lerp(Vector3.zero, _finalScale, progress);
            yield return null;
        }

        fruit.transform.localScale = _finalScale;
    }
}