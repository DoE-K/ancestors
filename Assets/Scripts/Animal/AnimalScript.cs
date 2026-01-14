using UnityEngine;

public abstract class AnimalScript : MonoBehaviour
{
    [Header("General Settings")]
    public string speciesName;
    public bool isMale;
    public bool isPredator;

    [Header("Stats")]
    public float speed = 3f;
    public Transform home;
    public bool isHome;

    public float health = 100;
    public float food = 100;
    public float drink = 100;

    [Header("Wandering")]
    public Vector2 moveDirection;
    public float moveTimer;
    public float waitTimer;
    public bool isMoving;
    public float minMoveTime = 1f;
    public float maxMoveTime = 3f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;
    public bool lookingRight = true;

    [Header("Chasing")]
    public bool isChasing = false;
    public bool isChased = false;
    public Transform theChasenOne;
    public bool touchingOtherAnimal;

    protected virtual void Start()
    {
        gameObject.name = speciesName;
        //isChasing = false;
        //isHome = false;
        ChooseNewDirection();
    }

    protected virtual void Update()
    {
        //FlipSystem();
        //WanderingAround();
        //Act();
    }

    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {

    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {

    }

    protected virtual void FlipSystem()
    {
        if (isMoving)
        {
            if (moveDirection.x < 0f && lookingRight == true)
            {
                Flip();
            }
            else if (moveDirection.x > 0f && lookingRight == false)
            {
                Flip();
            }

        }
    }

    protected virtual void Move(){
        Debug.Log("SorryFake");
    }

    protected virtual void WanderingAround()
    {
        if(isMoving)
        {
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
    }

    protected virtual void ReturnHome()
    {
        if(isHome != true)
        {
            isMoving = true;

            if (home == null) return;
            //if (isHome == true) return;

            moveDirection = (home.position - transform.position).normalized;
            transform.position += (Vector3)(moveDirection * speed * 2f * Time.deltaTime);
        }
        
        
    }

    public void Flip()
    {
        lookingRight = !lookingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void ChooseNewDirection()
    {
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

        moveTimer = Random.Range(minMoveTime, maxMoveTime);
        isMoving = true;
    }

    public virtual void PredatorAct()
    {
        FlipSystem();

        if (isChasing == true && theChasenOne != null)
        {
            Chasing();

        }
        else
        {
            WanderingAround();
        }

    }

    public virtual void RunAwayAct()
    {
        FlipSystem();

        if (isChased == true && home != null)
        {
            ReturnHome();

        }
        else
        {
            WanderingAround();
        }
    }
    

    public void Chasing()
    {
        isMoving = true;

        if (theChasenOne == null) return;

        moveDirection = (theChasenOne.position - transform.position).normalized;
        transform.position += (Vector3)(moveDirection * speed * 1.5f * Time.deltaTime);
    }

    /*public void Chased()
    {
        isMoving = true;

        if (theChasenOne == null) return;

        moveDirection = (theChasenOne.position - transform.position).normalized;
        transform.position += (Vector3)(moveDirection * speed * 1.5f * Time.deltaTime);
    }*/
}