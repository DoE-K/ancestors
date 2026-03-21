using UnityEngine;

public class AnimalSensor : MonoBehaviour
{
    [SerializeField] private float _scanInterval = 0.5f;

    public Transform NearestFood     { get; private set; }
    public Transform NearestWater    { get; private set; }
    public Transform NearestPredator { get; private set; }
    public Transform NearestHome     { get; private set; }
    public Transform NearestMate     { get; private set; }

    public bool CanSeeFood     => NearestFood     != null;
    public bool CanSeeWater    => NearestWater    != null;
    public bool CanSeePredator => NearestPredator != null;
    public bool CanSeeHome     => NearestHome     != null;
    public bool CanSeeMate     => NearestMate     != null;

    private AnimalData    _data;
    private Collider2D[]  _hitBuffer  = new Collider2D[16];
    private Animal        _selfAnimal;
    private AnimalMemory  _memory;

    public void Initialise(AnimalData data)
    {
        _data       = data;
        _selfAnimal = GetComponent<Animal>();
        _memory     = GetComponent<AnimalMemory>();
    }

    private void OnEnable()  => InvokeRepeating(nameof(Scan), 0f, _scanInterval);
    private void OnDisable() => CancelInvoke(nameof(Scan));

    private void Scan()
    {
        if (_data == null) return;

        NearestFood     = FindNearest(_data.foodTag);
        NearestWater    = FindNearest(_data.waterTag);
        NearestPredator = FindNearest(_data.predatorTag);
        NearestHome     = FindNearest(_data.homeTag);
        NearestMate     = FindNearestMate();

        // ── Write to memory whenever a resource is spotted ───────────────────
        if (_memory != null)
        {
            if (NearestFood  != null) _memory.RememberFood(NearestFood.position);
            if (NearestWater != null) _memory.RememberWater(NearestWater.position);
        }
    }

    private Transform FindNearest(string tag)
    {
        int count = Physics2D.OverlapCircleNonAlloc(
            transform.position, _data.sightRange, _hitBuffer);

        Transform nearest  = null;
        float     bestDist = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            var col = _hitBuffer[i];
            if (col == null) continue;
            if (col.GetComponent<Animal>() == _selfAnimal) continue;
            if (!col.CompareTag(tag)) continue;

            float dist = Vector2.Distance(transform.position, col.transform.position);
            if (dist < bestDist)
            {
                bestDist = dist;
                nearest  = col.transform;
            }
        }

        return nearest;
    }

    private Transform FindNearestMate()
    {
        int count = Physics2D.OverlapCircleNonAlloc(
            transform.position, _data.sightRange, _hitBuffer);

        Transform nearest  = null;
        float     bestDist = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            var col = _hitBuffer[i];
            if (col == null) continue;

            var other = col.GetComponent<Animal>();
            if (other == null || other == _selfAnimal) continue;
            if (other.Data.speciesName != _data.speciesName) continue;
            if (!other.Needs.IsReadyToReproduce) continue;

            float dist = Vector2.Distance(transform.position, col.transform.position);
            if (dist < bestDist)
            {
                bestDist = dist;
                nearest  = col.transform;
            }
        }

        return nearest;
    }

    private void OnDrawGizmosSelected()
    {
        if (_data == null) return;
        Gizmos.color = new Color(1f, 1f, 0f, 0.15f);
        Gizmos.DrawWireSphere(transform.position, _data.sightRange);
        Gizmos.color = new Color(0f, 1f, 0f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, _data.interactRange);
    }
}