using UnityEngine;

public class CurrantSpawnerScript : MonoBehaviour
{
    public GameObject currantPrefab;
    public int currantCount = 40;
    public Vector2 spawnAreaMin = new Vector2(-48, -48);
    public Vector2 spawnAreaMax = new Vector2(48, 48);

    private void Start()
    {
        SpawnCurrant();
        PlayerPrefs.SetInt("CurrantPlaced", 1);
    }

    void SpawnCurrant()
    {
        for (int i = 0; i < currantCount; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            Instantiate(currantPrefab, randomPosition, Quaternion.identity);
        }
    }
}
