/*using UnityEngine;

public class StoneSpawnerScript : MonoBehaviour
{
    public GameObject stonePrefab;
    public int stoneCount = 100;
    public Vector2 spawnAreaMin = new Vector2(-48, -48);
    public Vector2 spawnAreaMax = new Vector2(48, 48);

    private void Start()
    {
        SpawnStones();
        PlayerPrefs.SetInt("StonePlaced", 1);
    }

    void SpawnStones()
    {
        for (int i = 0; i < stoneCount; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            Instantiate(stonePrefab, randomPosition, Quaternion.identity);
        }
    }
}*/
