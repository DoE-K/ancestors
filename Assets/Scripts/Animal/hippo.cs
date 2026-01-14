using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hippo : AnimalScript
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

        if(other.CompareTag("Tiger"))
        {
            isChased = true;
        }

        if(other.CompareTag("Fish"))
        {
            isChasing = true;
            theChasenOne = other.transform;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Tiger"))
        {
            isChased = false;
        }

        if (other.CompareTag("Fish"))
        {
            isChasing = false;
            theChasenOne = null;
        }
    }
}
