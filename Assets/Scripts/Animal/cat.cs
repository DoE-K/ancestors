using UnityEngine;

public class cat : AnimalScript
{
    private Vector2 direction;

    protected override void Start()
    {
        //base.Start();
        //speciesName = "Deer";
        //isPredator = false;
        //PickNewDirection();
    }

    void Update()
    {
        Move();
    }

    /*protected override void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }*/

    public override void Act()
    {
        // z. B. "Gras fressen" oder stehen bleiben
    }
}
