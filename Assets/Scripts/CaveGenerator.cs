using System.Collections.Generic;
using UnityEngine;

public class CaveGenerator : MonoBehaviour
{
    [Header("Höhlen-Einstellungen")]
    public GameObject segmentPrefab;    // Tunnelstück-Prefab
    public Transform player;            // Spieler-Transform
    public int segmentsAhead = 2;       // Segmente, die immer vor Spieler aktiv sind
    public float segmentLength = 35f;   // Höhe eines Segments

    [Header("Erz-Prefabs")]
    public GameObject diamondPrefab;
    public GameObject goldPrefab;
    public GameObject stonePrefab;

    private float lastY = 0f;
    private Queue<GameObject> activeSegments = new Queue<GameObject>();

    void Start()
    {
        for(int i = 0; i < segmentsAhead; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        if(player.position.y - (25 * segmentsAhead) < lastY)
        {
            SpawnSegment();
        }
    }

    void SpawnSegment()
    {
        // Neues Segment instanziieren
        GameObject seg = Instantiate(segmentPrefab, new Vector3(0, lastY, 0), Quaternion.identity);
        lastY -= segmentLength;

        // Erze spawnen
        Segment segScript = seg.GetComponent<Segment>();
        foreach(Transform spawnPoint in segScript.orePoints)
        {
            float rand = Random.value; // 0 - 1
            if(rand < 0.05f) // 5% Diamant
                Instantiate(diamondPrefab, spawnPoint.position, Quaternion.identity, seg.transform);
            else if(rand < 0.2f) // 15% Gold
                Instantiate(goldPrefab, spawnPoint.position, Quaternion.identity, seg.transform);
            else if(rand < 0.6f) // 40% Stein
                Instantiate(stonePrefab, spawnPoint.position, Quaternion.identity, seg.transform);
            // Rest: leer lassen
        }

        activeSegments.Enqueue(seg);

        // Alte Segmente löschen
        if(activeSegments.Count > segmentsAhead + 2)
        {
            Destroy(activeSegments.Dequeue());
        }
    }
}
