/*using UnityEngine;

public class TreeSpawnerScript : MonoBehaviour
{
    public GameObject treePrefab;
    public int treeCount = 200;
    public Vector2 spawnAreaMin = new Vector2(-48, -48);
    public Vector2 spawnAreaMax = new Vector2(48, 48);

    private void Start()
    {
        SpawnTrees();
        PlayerPrefs.SetInt("TreesPlaced", 1);        
    }

    void SpawnTrees()
    {
        for (int i = 0; i < treeCount; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            Instantiate(treePrefab, randomPosition, Quaternion.identity);
        }
    }
}*/
