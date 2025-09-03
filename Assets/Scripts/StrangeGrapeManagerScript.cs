using UnityEngine;

public class StrangeGrapeSpawnerScript : MonoBehaviour
{
    public GameObject strangegrapePrefab;
    public int StrangeGrapeCount = 10;
    public Vector2 spawnAreaMin = new Vector2(-48, -48);
    public Vector2 spawnAreaMax = new Vector2(48, 48);

    private void Start()
    {
        SpawnStrangeGrape();
        PlayerPrefs.SetInt("StrangeGrapePlaced", 1);
    }

    void SpawnStrangeGrape()
    {
        for (int i = 0; i < StrangeGrapeCount; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            Instantiate(strangegrapePrefab, randomPosition, Quaternion.identity);
        }
    }
}
