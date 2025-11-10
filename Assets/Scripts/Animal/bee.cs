using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bee : AnimalScript
{
    //public float speed = 2f;           // Fluggeschwindigkeit
    //public float moveRange = 3f;       // Wie weit sie in eine Richtung fliegt, bevor sie umdreht
    //public float bobAmplitude = 0.2f;  // Wie stark sie auf/ab schwebt
    //public float bobFrequency = 2f;  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /*protected override void Move()
    {
        /*if (isMoving)
        {
            if (moveDirection.x < 0f && transform.localScale.x > 0f)
            {
                Debug.Log("Bewegt sich nach links!");
                Flip();
                //transform.Translate(moveDirection * speed * Time.deltaTime);
                //Flip();
            }
            else if (moveDirection.x > 0f && transform.localScale.x < 0f)
            {
                Flip();
                //transform.Translate(moveDirection * speed * Time.deltaTime);
            }

            transform.Translate(moveDirection * speed * Time.deltaTime);

            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0f)
            {
                isMoving = false;
                waitTimer = Random.Range(minWaitTime, maxWaitTime);
            }
        }
        else
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                ChooseNewDirection();
            }
        }
    }*/


    public override void Act()
    {
        // z. B. "Gras fressen" oder stehen bleiben
    }
}
