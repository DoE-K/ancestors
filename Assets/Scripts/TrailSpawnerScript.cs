using UnityEngine;

public class TrailSpawnerScript : MonoBehaviour
{
    public GameObject trailPrefab;       // Dein Fußspur-Objekt
    public float spawnInterval = 0.01f;   // Wie oft Spuren erscheinen
    private float timer = 0f;

    private Vector2 lastPosition;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && (Vector2)transform.position != lastPosition)
        {
            Instantiate(trailPrefab, transform.position, Quaternion.identity);
            lastPosition = transform.position;
            timer = 0f;
        }
    }
}
