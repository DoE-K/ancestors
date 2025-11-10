using UnityEngine;

public class tiger : AnimalScript
{
    private Transform target;

    protected override void Start()
    {
        //base.Start();
        //speciesName = "Wolf";
        isPredator = true;
    }

    void Update()
    {
        Move();
    }

    /*protected override void Move()
    {
        if (target != null)
        {
            Vector2 dir = (target.position - transform.position).normalized;
            transform.Translate(dir * speed * Time.deltaTime);
        }
        else
        {
            SearchForPrey();
        }
    }*/

    private void SearchForPrey()
    {
        
    }

    public override void Act()
    {
        Debug.Log("hey");
    }
}
