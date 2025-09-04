using System.Collections.Generic;
using UnityEngine;

public class CaveGenerator : MonoBehaviour
{
    [Header("Höhlen-Einstellungen")]
    public GameObject segmentPrefab;    
    public Transform player;            
    public int segmentsAhead = 2;       
    public float segmentLength = 35f;   

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
        
        GameObject seg = Instantiate(segmentPrefab, new Vector3(0, lastY, 0), Quaternion.identity);
        lastY -= segmentLength;

        
        Segment segScript = seg.GetComponent<Segment>();
        foreach(Transform spawnPoint in segScript.orePoints)
        {
            float rand = Random.value; 
            if(rand < 0.05f) 
                Instantiate(diamondPrefab, spawnPoint.position, Quaternion.identity, seg.transform);
            else if(rand < 0.2f) 
                Instantiate(goldPrefab, spawnPoint.position, Quaternion.identity, seg.transform);
            else if(rand < 0.6f) 
                Instantiate(stonePrefab, spawnPoint.position, Quaternion.identity, seg.transform);
            
        }

        activeSegments.Enqueue(seg);

        
        if(activeSegments.Count > segmentsAhead + 2)
        {
            Destroy(activeSegments.Dequeue());
        }
    }
}
