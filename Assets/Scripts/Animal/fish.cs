using UnityEngine;

public class fish : AnimalScript
{
    void Start()
    {
        isHome = false;
    }

    void Update()
    {
        ReturnHome();
        PredatorAct();
        RunAwayAct();
    
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Home"))
        {
            Debug.Log("ENter");
            isHome = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Home"))
        {
            isHome = false;
        }
    }
}
