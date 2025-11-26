using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryGenerator : MonoBehaviour
{
    [Header("Berry-Prefabs")]
    public GameObject berry1Prefab;
    public GameObject berry2Prefab;
    public GameObject berry3Prefab;

    [Header("Bush Settings")]
    public Transform berPoints; 
    public string berryType = "random"; 

    [Header("Growth Settings")]
    public float growDuration = 2f; 
    public Vector3 finalScale = Vector3.one;

    [Header("Spawn Delay Settings")]
    public float minGrowDelay = 1f;
    public float maxGrowDelay = 5f;

    void Start()
    {
        SpawnBerries();
    }

    void SpawnBerries()
    {
        foreach (Transform spawnPoint in berPoints)
        {
            StartCoroutine(CheckAndSpawnBerry(spawnPoint));
        }
    }

    IEnumerator CheckAndSpawnBerry(Transform spawnPoint)
    {
        while (true)
        {
            if (spawnPoint.childCount == 0)
            {
                GameObject prefabToSpawn = GetBerryPrefab();
                if (prefabToSpawn != null)
                {
                    GameObject berry = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity, spawnPoint);
                    berry.transform.localScale = Vector3.zero;

                    float randomDelay = Random.Range(minGrowDelay, maxGrowDelay);
                    StartCoroutine(GrowBerry(berry, randomDelay));
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    GameObject GetBerryPrefab()
    {
        string typeToUse = berryType;
        if (berryType == "random")
        {
            float rand = Random.value;
            if (rand < 0.05f) typeToUse = "diamond";
            else if (rand < 0.2f) typeToUse = "gold";
            else typeToUse = "stone";
        }

        switch (typeToUse)
        {
            case "diamond": return berry1Prefab;
            case "gold": return berry2Prefab;
            case "stone": return berry3Prefab;
            default: return null;
        }
    }

    IEnumerator GrowBerry(GameObject berry, float delay)
    {
        // Warte vor dem Wachstum
        yield return new WaitForSeconds(delay);

        float t = 0f;
        while (t < growDuration)
        {
            t += Time.deltaTime;
            float progress = t / growDuration;
            berry.transform.localScale = Vector3.Lerp(Vector3.zero, finalScale, progress);
            yield return null;
        }

        berry.transform.localScale = finalScale;
    }
}
