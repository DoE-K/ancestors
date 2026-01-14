using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elefant : AnimalScript
{

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

        if(other.CompareTag("Tiger")){
            isChasing = true;
            theChasenOne = other.transform;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Tiger")){
            isChasing = false;
            theChasenOne = null;
        }
    }

    
}
