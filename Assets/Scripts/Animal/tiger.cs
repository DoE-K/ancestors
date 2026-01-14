using UnityEngine;

public class tiger : AnimalScript
{
    public Collider2D body;

    void Start()
    {
        isHome = false;
    }

    void Update()
    {
        PredatorAct();
        RunAwayAct();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Home"))
        {
            isChased = false;
            isHome = true;
        }

        if(other.CompareTag("Mamooth"))
        {
            isChased = true;
        }

        if(other.CompareTag("Turtle"))
        {
            isChasing = true;
            theChasenOne = other.transform;
        }

        if(other.CompareTag("Hippo")){
            isChasing = true;
            theChasenOne = other.transform;
        }

        if(other.CompareTag("Fish")){
            isChasing = true;
            theChasenOne = other.transform;
        }

    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Mamooth"))
        {
            isChased = false;
        }

        if (other.CompareTag("Turtle"))
        {
            isChasing = false;
            theChasenOne = null;
        }

        if(other.CompareTag("Hippo")){
            isChasing = false;
            theChasenOne = null;
        }

        if(other.CompareTag("Fish")){
            isChasing = false;
            isChased = false;
            theChasenOne = null;
        }
    }
}
